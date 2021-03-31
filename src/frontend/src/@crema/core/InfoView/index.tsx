import React from 'react';
import {Loader, MessageView} from "../..";
import {useInfoViewContext} from './InfoViewContext';

interface InfoViewProps {}

const InfoView: React.FC<InfoViewProps> = () => {
  const {error, loading, message} = useInfoViewContext();

  const showMessage = () => <MessageView variant='success' message={message.toString()} />;

  const showError = () => <MessageView variant='error' message={error.toString()} />;

  return (
    <>
      {loading && <Loader />}

      {message && showMessage()}
      {error && showError()}
    </>
  );
};

export default InfoView;
