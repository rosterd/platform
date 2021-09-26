import React, {ReactNode, useState} from 'react';
import {Button, Dialog, DialogActions, DialogContent, DialogTitle, LinearProgress, makeStyles} from '@material-ui/core';
import {Formik, Form} from 'formik';
import {Schema} from 'yup';
import Alert from '@material-ui/lab/Alert';
import {CremaTheme} from 'types/AppContextPropsType';

const useStyles = makeStyles((theme: CremaTheme) => ({
  alert: {
    marginBottom: theme.spacing(4),
  },
}));

export interface FormProps<T> {
  onSubmit: (T) => void;
  initialValues: T;
  validationSchema: Schema<any, any>;
}

export interface ModalProps {
  open: boolean;
  onClose: () => void;
  title: string;
  submitButtonLabel: string;
  closeAfter?: boolean;
  successMessage?: string;
}

interface Props<T> {
  modalProps: ModalProps;
  formProps: FormProps<T>;
}

type PropsWithChildren<P> = P & {children?: ReactNode};

const FormModal = (props: PropsWithChildren<Props<Record<string, any>>>): JSX.Element => {
  const classes = useStyles();
  const {modalProps, formProps, children} = props;
  const {open, onClose, submitButtonLabel = 'Submit', title, closeAfter = false, successMessage = 'Succefully created'} = modalProps;
  const {onSubmit, initialValues, validationSchema} = formProps;
  const [showSuccess, setShowSuccess] = useState(false);
  const [errors, setErrors] = useState([]);

  const resetErrors = () => {
    setErrors([]);
    setShowSuccess(false);
  };

  return (
    <Formik
      enableReinitialize
      initialValues={initialValues}
      validationSchema={validationSchema}
      onSubmit={async (values, {setSubmitting, resetForm}) => {
        setShowSuccess(false);
        setSubmitting(true);
        try {
          await onSubmit(values);
          setShowSuccess(true);
          setTimeout(() => {
            setShowSuccess(false);
            if (closeAfter) onClose();
          }, 2000);
          resetForm();
        } catch (e) {
          setErrors(e?.Messages || []);
          setSubmitting(false);
        }
      }}>
      {({submitForm, isSubmitting, resetForm}) => (
        <Dialog
          fullWidth
          maxWidth='sm'
          open={open}
          onClose={() => {
            onClose();
            resetErrors();
            resetForm();
          }}
          aria-labelledby='form-dialog-title'>
          <DialogTitle id='form-dialog-title'>{title}</DialogTitle>
          <DialogContent>
            {showSuccess ? (
              <Alert severity='success' className={classes.alert}>
                {successMessage}
              </Alert>
            ) : null}
            {!!errors.length && (
              <Alert severity='error' className={classes.alert}>
                {errors.map((error, index) => (
                  // eslint-disable-next-line react/no-array-index-key
                  <div key={index}>{error}</div>
                ))}
              </Alert>
            )}
            <Form>{children}</Form>
            {isSubmitting && <LinearProgress />}
          </DialogContent>
          <DialogActions>
            <Button
              onClick={() => {
                onClose();
                resetForm();
                resetErrors();
              }}
              color='primary'>
              Close
            </Button>
            <Button onClick={submitForm} color='primary' disabled={isSubmitting} variant='contained'>
              {submitButtonLabel}
            </Button>
          </DialogActions>
        </Dialog>
      )}
    </Formik>
  );
};

export default FormModal;
