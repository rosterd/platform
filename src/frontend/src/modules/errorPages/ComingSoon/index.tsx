/* eslint-disable react/jsx-props-no-spreading */
import React from 'react';
import Button from '@material-ui/core/Button';
import TextField from '@material-ui/core/TextField';
import {Form, Formik, useField} from 'formik';
import * as yup from 'yup';
import InfoView from '@crema/core/InfoView';
import Box from '@material-ui/core/Box';
import Typography from '@material-ui/core/Typography';
import {grey} from '@material-ui/core/colors';
import {makeStyles} from '@material-ui/core';
import {useIntl} from 'react-intl';
import {Fonts} from '../../../shared/constants/AppEnums';
import AppAnimate from '../../../@crema/core/AppAnimate';
import IntlMessages from '../../../@crema/utility/IntlMessages';
import {
  showMessage,
  useInfoViewActionsContext,
} from '../../../@crema/core/InfoView/InfoViewContext';

const useStyles = makeStyles(() => ({
  form: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    marginBottom: 12,
  },
  textField: {
    width: '100%',
    marginBottom: 20,
  },
  button: {
    fontWeight: Fonts.BOLD,
    fontSize: 16,
    textTransform: 'capitalize',
  },
}));
const MyTextField = (props: any) => {
  const [field, meta] = useField(props);
  const errorText = meta.error && meta.touched ? meta.error : '';
  // eslint-disable-next-line react/jsx-props-no-spreading
  return (
    <TextField
      {...props}
      {...field}
      helperText={errorText}
      error={!!errorText}
    />
  );
};

const validationSchema = yup.object({
  email: yup
    .string()
    .email('The Email you entered is not a valid format!')
    .required('Please enter Email Address!'),
});

const ComingSoon = (): JSX.Element => {
  const dispatch = useInfoViewActionsContext()!;

  const classes = useStyles();
  const {messages} = useIntl();

  return (
    <AppAnimate animation='transition.slideUpIn' delay={200}>
      <Box
        py={{xl: 8}}
        flex={1}
        display='flex'
        flexDirection='column'
        justifyContent='center'
        alignItems='center'
        textAlign='center'>
        <Box>
          <Box
            component='h3'
            mb={{xs: 4, xl: 10}}
            fontSize={{xs: 20, md: 24}}
            fontWeight={Fonts.BOLD}>
            <IntlMessages id='error.comingSoon' />!
          </Box>
          <Box
            mb={{xs: 5, xl: 12}}
            color={grey[600]}
            fontWeight={Fonts.MEDIUM}
            fontSize={16}>
            <Typography>
              <IntlMessages id='error.comingSoonMessage1' />
            </Typography>
            <Typography>
              <IntlMessages id='error.comingSoonMessage2' />
            </Typography>
          </Box>
          <Box mx='auto' mb={5} maxWidth={384}>
            <Formik
              validateOnChange
              initialValues={{
                email: '',
              }}
              validationSchema={validationSchema}
              onSubmit={(data, {resetForm}) => {
                dispatch(
                  showMessage(
                    messages['error.comingSoonNotification'] as string,
                  ),
                );
                resetForm();
              }}>
              {() => (
                <Form className={classes.form}>
                  <MyTextField
                    placeholder='Email'
                    name='email'
                    label={<IntlMessages id='common.emailAddress' />}
                    className={classes.textField}
                    variant='outlined'
                  />

                  <Button
                    variant='contained'
                    color='primary'
                    type='submit'
                    className={classes.button}>
                    <IntlMessages id='error.notifyMe' />
                  </Button>
                </Form>
              )}
            </Formik>
          </Box>
          <Box mb={5} maxWidth={{xs: 300, sm: 400, xl: 672}} width='100%'>
            <img
              src='/assets/images/errorPageImages/comingsoon.png'
              alt='404'
            />
          </Box>
        </Box>
        <InfoView />
      </Box>
    </AppAnimate>
  );
};

export default ComingSoon;
