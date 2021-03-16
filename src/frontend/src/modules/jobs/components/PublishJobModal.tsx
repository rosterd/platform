import React from 'react';
import Dialog from '@material-ui/core/Dialog';
import DialogActions from '@material-ui/core/DialogActions';
import DialogContent from '@material-ui/core/DialogContent';
import DialogTitle from '@material-ui/core/DialogTitle';
import {Formik, Form, Field} from 'formik';
import {Button, LinearProgress} from '@material-ui/core';
import {TextField} from 'formik-material-ui';
import {DateTimePicker} from 'formik-material-ui-pickers';
import {MuiPickersUtilsProvider} from '@material-ui/pickers';
import DateFnsUtils from '@date-io/date-fns';

interface PublishJobModalProps {
  open: boolean;
  handleClose: () => void;
}

interface FormValues {
  title: string;
  skill: string;
  description: string;
  facility: string;
  from: string;
  to: string;
  comments: string;
}

export default function PublishJobModal(props: PublishJobModalProps) {
  return (
    <MuiPickersUtilsProvider utils={DateFnsUtils}>
      <Formik
        initialValues={{
          title: '',
          skill: '',
          description: '',
          facility: '',
          from: '',
          to: '',
          comments: '',
        }}
        validate={(values) => {
          const errors: Partial<FormValues> = {};
          if (!values.skill) {
            errors.skill = 'Required';
          }
          return errors;
        }}
        onSubmit={(values, {setSubmitting}) => {
          setTimeout(() => {
            setSubmitting(false);
            alert(JSON.stringify(values, null, 2));
          }, 500);
        }}>
        {({submitForm, isSubmitting}) => (
          <Dialog
            fullWidth
            maxWidth='sm'
            open={props.open}
            onClose={props.handleClose}
            aria-labelledby='form-dialog-title'>
            <DialogTitle id='form-dialog-title'>Publish Job</DialogTitle>
            <DialogContent>
              <Form>
                <Field
                  component={TextField}
                  name='title'
                  label='Title'
                  fullWidth
                />
                <br />
                <Field
                  component={TextField}
                  name='skill'
                  label='Skill Required'
                  fullWidth
                />
                <br />
                <Field
                  component={TextField}
                  name='description'
                  label='Description'
                  fullWidth
                />
                <br />
                <Field
                  component={TextField}
                  name='facility'
                  label='Facility'
                  fullWidth
                />
                <br />
                <Field
                  component={DateTimePicker}
                  name='from'
                  label='From'
                  fullWidth
                />
                <br />
                <Field
                  component={DateTimePicker}
                  name='to'
                  label='To'
                  fullWidth
                />

                <Field
                  component={TextField}
                  name='comments'
                  label='Comments'
                  fullWidth
                />
                <br />
                {isSubmitting && <LinearProgress />}
              </Form>
            </DialogContent>
            <DialogActions>
              <Button onClick={props.handleClose} color='primary'>
                Cancel
              </Button>
              <Button
                onClick={submitForm}
                color='primary'
                disabled={isSubmitting}
                variant='contained'>
                Publish
              </Button>
            </DialogActions>
          </Dialog>
        )}
      </Formik>
    </MuiPickersUtilsProvider>
  );
}
