import React, {useContext} from 'react';
import TextField from '@material-ui/core/TextField';
import Button from '@material-ui/core/Button';
import {Checkbox} from '@material-ui/core';
import {Form, Formik, useField} from 'formik';
import * as yup from 'yup';
import InfoView from '../../../@crema/core/InfoView';
import Box from '@material-ui/core/Box';
import IntlMessages from '../../../@crema/utility/IntlMessages';
import {useIntl} from 'react-intl';
import {makeStyles} from '@material-ui/core/styles';
import clsx from 'clsx';
import {Link, useHistory} from 'react-router-dom';
import {AuthType, Fonts} from 'shared/constants/AppEnums';
import AppContextPropsType, {
  CremaTheme,
} from '../../../types/AppContextPropsType';
import {
  fetchError,
  fetchStart,
  fetchSuccess,
  useInfoViewActionsContext,
} from '../../../@crema/core/InfoView/InfoViewContext';
import {fetchDataApi, saveDataApi} from '../../../@crema/utility/ApiHook';
import {setAuthToken} from '../../../@crema/services/ApiConfig';
import {defaultUser} from '../../../shared/constants/AppConst';
import AppContext from '../../../@crema/utility/AppContext';

const useStyles = makeStyles((theme: CremaTheme) => ({
  formRoot: {
    textAlign: 'left',
    [theme.breakpoints.up('xl')]: {
      marginBottom: 24,
    },
  },
  myTextFieldRoot: {
    width: '100%',
  },
  checkboxRoot: {
    marginLeft: -12,
  },
  pointer: {
    cursor: 'pointer',
  },
  btnRoot: {
    borderRadius: theme.overrides.MuiCard.root.borderRadius,
    width: '10rem',
    fontWeight: Fonts.REGULAR,
    fontSize: 16,
    textTransform: 'capitalize',
  },
  btnRootFull: {
    width: '100%',
  },
  dividerRoot: {
    marginBottom: 16,
    marginLeft: -48,
    marginRight: -48,
    [theme.breakpoints.up('xl')]: {
      marginBottom: 32,
    },
  },
  textPrimary: {
    color: theme.palette.text.primary,
  },
  colorTextPrimary: {
    color: theme.palette.primary.main,
  },
  underlineNone: {
    textDecoration: 'none',
  },
  textGrey: {
    color: theme.palette.grey[500],
  },
}));
const MyTextField = (props: any) => {
  const [field, meta] = useField(props);
  const errorText = meta.error && meta.touched ? meta.error : '';
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
  email: yup.string().email('Invalid Email').required('Email required'),
  password: yup.string().required('Password required'),
});

interface UserSigninProps {}

const SigninJwtAuth: React.FC<UserSigninProps> = (props) => {
  const dispatch = useInfoViewActionsContext()!;
  const {updateAuthUser} = useContext<AppContextPropsType>(AppContext);
  const history = useHistory();

  const onGoToForgetPassword = () => {
    history.push('/forget-password');
  };

  const {messages} = useIntl();

  const classes = useStyles(props);

  const onSigninUser = (data: {email: string; password: string}) => {
    dispatch(fetchStart());
    saveDataApi(`auth`, data)
      .then((data: any) => {
        console.log('data: ', data);
        setAuthToken(data.token);
        fetchDataApi('auth')
          .then((data: any) => {
            console.log('data: ', data);
            updateAuthUser({
              authType: AuthType.JWT_AUTH,
              displayName: data.name,
              email: data.email,
              role: defaultUser.role,
              token: data._id,
              uid: data._id,
              photoURL: data.avatar,
            });
            dispatch(fetchSuccess());
          })
          .catch((error) => dispatch(fetchError(error.message)));
      })
      .catch((error) => dispatch(fetchError(error.message)));
  };
  return (
    <Box flex={1} display='flex' flexDirection='column'>
      <Box
        px={{xs: 6, sm: 10, xl: 15}}
        pt={8}
        flex={1}
        display='flex'
        flexDirection='column'>
        <Formik
          validateOnChange={true}
          initialValues={{
            email: 'crema.demo@gmail.com',
            password: 'Pass@1!@all',
          }}
          validationSchema={validationSchema}
          onSubmit={(data, {setSubmitting}) => {
            setSubmitting(true);
            onSigninUser({email: data.email, password: data.password});
            setSubmitting(false);
          }}>
          {({isSubmitting}) => (
            <Form className={classes.formRoot} noValidate autoComplete='off'>
              <Box mb={{xs: 5, xl: 8}}>
                <MyTextField
                  placeholder={messages['common.email']}
                  name='email'
                  label={<IntlMessages id='common.email' />}
                  variant='outlined'
                  className={classes.myTextFieldRoot}
                />
              </Box>

              <Box mb={{xs: 3, lg: 4}}>
                <MyTextField
                  type='password'
                  placeholder={messages['common.password']}
                  label={<IntlMessages id='common.password' />}
                  name='password'
                  variant='outlined'
                  className={classes.myTextFieldRoot}
                />
              </Box>

              <Box
                mb={{xs: 3, xl: 4}}
                display='flex'
                flexDirection={{xs: 'column', sm: 'row'}}
                alignItems={{sm: 'center'}}
                justifyContent={{sm: 'space-between'}}
                fontSize={15}>
                <Box display='flex' alignItems='center'>
                  <Checkbox className={classes.checkboxRoot} />
                  <Box className={classes.textGrey} component='span'>
                    <IntlMessages id='common.rememberMe' />
                  </Box>
                </Box>
                <Box
                  color='primary.main'
                  component='span'
                  ml={{sm: 4}}
                  className={classes.pointer}
                  onClick={onGoToForgetPassword}
                  fontSize={15}>
                  <IntlMessages id='common.forgetPassword' />
                </Box>
              </Box>

              <Box
                mb={6}
                display='flex'
                flexDirection={{xs: 'column', sm: 'row'}}
                alignItems={{sm: 'center'}}
                justifyContent={{sm: 'space-between'}}>
                <Button
                  variant='contained'
                  color='primary'
                  type='submit'
                  disabled={isSubmitting}
                  className={classes.btnRoot}>
                  <IntlMessages id='common.login' />
                </Button>

                <Box
                  ml={{xs: 0, sm: 4}}
                  mt={{xs: 3, sm: 0}}
                  color='text.secondary'
                  fontSize={15}>
                  <Box className={classes.textGrey} component='span' mr={2}>
                    <IntlMessages id='common.dontHaveAccount' />
                  </Box>
                  <Box component='span'>
                    <Link
                      to='/signup'
                      className={clsx(
                        classes.underlineNone,
                        classes.colorTextPrimary,
                      )}>
                      <IntlMessages id='common.signup' />
                    </Link>
                  </Box>
                </Box>
              </Box>
            </Form>
          )}
        </Formik>
      </Box>

      <InfoView />
    </Box>
  );
};

export default SigninJwtAuth;