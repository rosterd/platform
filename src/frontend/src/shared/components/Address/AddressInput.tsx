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

export type AddressFieldType = {
  address: string;
  country: string;
  city: string;
  suburb: string | null | undefined;
  latitude: number;
  longitude: number;
};

export const addressInitialValue: AddressFieldType = {
  latitude: 0,
  longitude: 0,
  suburb: '',
  city: '',
  country: '',
  address: '',
};

export const AddressInput = ({name, label, isRequired = false}: AddressInputType): JSX.Element => {
  const [{value}, meta, {setValue, setTouched}] = useField<AddressFieldType>({
    name,
    validate: (inputValue) => {
      let error;
      if (isRequired && !inputValue.address) {
        error = `${label} is required`;
      }
      return error;
    },
  });
  const autocompleteService = new window.google.maps.places.AutocompleteService();

  const geoCoderService = new window.google.maps.Geocoder();
  const classes = useStyles();
  const [inputValue, setInputValue] = React.useState('');
  const [options, setOptions] = React.useState<google.maps.places.AutocompletePrediction[]>([]);

  const [prediction, setPrediction] = React.useState<google.maps.places.AutocompletePrediction | null>(null);

  const fetch = React.useMemo(
    () =>
      throttle((request: {input: string}, callback) => {
        autocompleteService?.getPlacePredictions(request, callback);
      }, 200),
    [],
  );

  // Reset form
  React.useEffect(() => {
    setInputValue(value.address);
  }, [value]);

  React.useEffect(() => {
    if (inputValue === '') {
      setOptions([]);
      setPrediction(null);
      return;
    }
    fetch({input: inputValue}, (results: google.maps.places.AutocompletePrediction[]) => {
      let newOptions: google.maps.places.AutocompletePrediction[] = [];

      if (prediction) {
        newOptions = [prediction];
      }

      if (results) {
        newOptions = [...newOptions, ...results];
      }
      setOptions(newOptions);
    });
  }, [inputValue, fetch]);

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
        value={prediction}
        inputValue={inputValue}
        onChange={(_, selectedPrediction: google.maps.places.AutocompletePrediction | null) => {
          setOptions(selectedPrediction ? [selectedPrediction, ...options] : options);
          geoCoderService?.geocode({placeId: selectedPrediction?.place_id}, (result) => {
            if (result?.length) {
              const country = result[0].address_components.find((address) => address.types.includes('country'))?.long_name || '';
              const city = result[0].address_components.find((address) => address.types.includes('administrative_area_level_1'))?.long_name || '';
              const suburb = result[0].address_components.find((address) => address.types.includes('sublocality'))?.long_name || '';
              const address = result[0].formatted_address || '';
              setPrediction(selectedPrediction);
              setValue({
                address,
                country,
                city,
                suburb,
                latitude: result[0].geometry.location.lat(),
                longitude: result[0].geometry.location.lng(),
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
