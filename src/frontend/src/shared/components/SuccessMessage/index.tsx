import React, {useState, useEffect} from 'react';
import Alert from '@material-ui/lab/Alert';

interface Props {
  children: React.ReactNode;
}
const SuccessMessage = ({children}: Props): JSX.Element | null => {
  const [show, setShow] = useState(true);

  useEffect(() => {
    const timeId = setTimeout(() => {
      setShow(false);
    }, 2000);

    return () => {
      clearTimeout(timeId);
    };
  }, []);

  if (!show) {
    return null;
  }

  return <Alert severity='success'>{children}</Alert>;
};

export default SuccessMessage;
