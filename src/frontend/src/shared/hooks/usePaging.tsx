import {AxiosRequestConfig} from 'axios';
import {useState, useEffect} from 'react';
import useRequest from './useRequest';

interface HookResponse {
  handlePageChange: (page: number, pageSize: number) => void;
  currentPage: number;
  totalCount: number;
  loading: boolean;
  deleteData: (id?: number, data?: any) => Promise<void>;
  setLoading: (boolean) => void;
  fetchData: (config: AxiosRequestConfig) => Promise<void>;
}

interface PagingResponse {
  currentPage?: number;
  totalCount?: number;
}

function usePaging<N, T extends PagingResponse & {items?: N[] | null}>(
  baseUrl: string,
  itemId: string,
): HookResponse & {items: N[]; addData: (data: N) => Promise<void>; updateData: (data: N) => Promise<void>} {
  const [results, setResults] = useState({} as T);
  const [totalCount, setTotalCount] = useState(0);
  const [items, setItems] = useState([] as N[]);
  const [loading, setLoading] = useState(false);
  const {requestMaker} = useRequest();

  const fetchData = async (config: AxiosRequestConfig) => {
    setLoading(true);
    const response = await requestMaker<T>(config);
    setLoading(false);
    setResults(response);
    setTotalCount(response.totalCount || 0);
    setItems(response?.items || []);
  };

  const addData = async (data: N) => {
    setLoading(true);
    const item = await requestMaker<N>({url: baseUrl, method: 'POST', data});
    setLoading(false);
    setItems([item, ...(items || [])]);
    setTotalCount((count) => count + 1);
  };

  const updateData = async (data: N) => {
    setLoading(true);
    const updatedItem = await requestMaker<N>({url: baseUrl, method: 'PUT', data});
    setLoading(false);
    setItems(items.map((item) => (item[itemId] === updatedItem[itemId] ? updatedItem : item)));
  };

  const deleteData = async (id?: number | null, body?: any) => {
    if (!id) return;
    setLoading(true);
    const requestConfig = {url: `${baseUrl}/${id}`, method: 'DELETE'} as AxiosRequestConfig;
    if (body) requestConfig.data = body;
    await requestMaker<N>(requestConfig);
    setLoading(false);
    setItems(items.filter((item) => item[itemId] !== id));
    setTotalCount((count) => count - 1);
  };

  useEffect(() => {
    (async () => {
      await fetchData({url: baseUrl});
    })();
  }, [baseUrl]);

  const handlePageChange = async (page: number, pageSize: number) => {
    const requestUrl = baseUrl.match(/\?/)
      ? `${baseUrl}&pageNumber=${page + 1}&pageSize=${pageSize}`
      : `${baseUrl}?pageNumber=${page + 1}&pageSize=${pageSize}`;
    await fetchData({url: requestUrl});
  };

  return {
    handlePageChange,
    currentPage: (results.currentPage || 1) - 1,
    totalCount,
    items,
    loading,
    fetchData,
    addData,
    updateData,
    deleteData,
    setLoading,
  };
}

export default usePaging;
