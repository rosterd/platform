import React from 'react';
import TextField from '@material-ui/core/TextField';
import Autocomplete from '@material-ui/lab/Autocomplete';
import LocationOnIcon from '@material-ui/icons/LocationOn';
import Grid from '@material-ui/core/Grid';
import Typography from '@material-ui/core/Typography';
import {makeStyles} from '@material-ui/core/styles';
import parse from 'autosuggest-highlight/parse';
import throttle from 'lodash/throttle';
import {useField} from 'formik';
import {AddressInputType} from '.';

const useStyles = makeStyles((theme) => ({
  icon: {
    color: theme.palette.text.secondary,
    marginRight: theme.spacing(2),
  },
}));

type FieldType = {
  prediction: google.maps.places.AutocompletePrediction | null;
  lat: number;
  lng: number;
} | null;

export const AddressInput = ({name, label}: AddressInputType): JSX.Element => {
  const [{value}, meta, {setValue, setTouched}] = useField<FieldType>(name);
  const autocompleteService = new window.google.maps.places.AutocompleteService();
  const geoCoderService = new window.google.maps.Geocoder();
  const classes = useStyles();
  const [inputValue, setInputValue] = React.useState('');
  const [options, setOptions] = React.useState<google.maps.places.AutocompletePrediction[]>([]);

  const fetch = React.useMemo(
    () =>
      throttle((request: {input: string}, callback) => {
        autocompleteService?.getPlacePredictions(request, callback);
      }, 200),
    [],
  );

  React.useEffect(() => {
    if (inputValue === '') {
      setOptions(value?.prediction ? [value.prediction] : []);
      setValue(null);
      return;
    }
    fetch({input: inputValue}, (results: google.maps.places.AutocompletePrediction[]) => {
      let newOptions: google.maps.places.AutocompletePrediction[] = [];

      if (value?.prediction) {
        newOptions = [value.prediction];
      }

      if (results) {
        newOptions = [...newOptions, ...results];
      }
      setOptions(newOptions);
    });
  }, [value, inputValue, fetch]);

  return (
    <>
      <Autocomplete
        id='address-autocomplete'
        style={{width: '100%'}}
        getOptionSelected={(option, selectedValue) => option.place_id === selectedValue.place_id}
        getOptionLabel={(option) => (typeof option === 'string' ? option : option.description)}
        filterOptions={(x) => x}
        options={options}
        autoComplete
        includeInputInList
        filterSelectedOptions
        value={value?.prediction || null}
        onChange={(event, prediction: google.maps.places.AutocompletePrediction | null) => {
          setOptions(prediction ? [prediction, ...options] : options);
          geoCoderService?.geocode({placeId: prediction?.place_id}, (result) => {
            if (result?.length) {
              setValue({
                prediction,
                lat: result[0].geometry.location.lat(),
                lng: result[0].geometry.location.lng(),
              });
            }
          });
        }}
        onInputChange={(_, newInputValue) => {
          setInputValue(newInputValue);
        }}
        renderInput={(params) => (
          <TextField
            onBlur={() => setTouched(true)}
            error={meta.touched && !!meta.error}
            helperText={meta.touched && meta.error}
            {...params}
            label={label}
            variant='standard'
            fullWidth
          />
        )}
        renderOption={(option) => {
          const matches = option.structured_formatting.main_text_matched_substrings;
          const parts = parse(
            option.structured_formatting.main_text,
            matches.map((match) => [match.offset, match.offset + match.length]),
          );

          return (
            <Grid container alignItems='center'>
              <Grid item>
                <LocationOnIcon className={classes.icon} />
              </Grid>
              <Grid item xs>
                {parts.map((part) => (
                  <span key={part.text} style={{fontWeight: part.highlight ? 700 : 400}}>
                    {part.text}
                  </span>
                ))}
                <Typography variant='body2' color='textSecondary'>
                  {option.structured_formatting.secondary_text}
                </Typography>
              </Grid>
            </Grid>
          );
        }}
      />
    </>
  );
};

export default AddressInput;
