import React from 'react';
import {Wrapper} from '@googlemaps/react-wrapper';
import AddressInput from './AddressInput';

export type AddressInputType = {
  label: string;
  name: string;
};

const index = (props: AddressInputType): JSX.Element => {
  const apiKey = process.env.REACT_APP_GOOGLE_API_KEY as string;
  return (
    <Wrapper apiKey={apiKey} libraries={['places']}>
      <AddressInput {...props} />
    </Wrapper>
  );
};

export default index;
