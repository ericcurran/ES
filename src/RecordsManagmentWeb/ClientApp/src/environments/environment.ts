// This file can be replaced during build by using the `fileReplacements` array.
// `ng build ---prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  azureAdTenantId: '1ad5568a-c025-4749-b1d4-868d82da39eb',

  azureAdClientId: 'cee2d7b1-2c63-4343-8272-d72c22e40caa',
  webApiEndpoint: 'https://localhost:5001/api/',
};

/*
 * In development mode, to ignore zone related error stack frames such as
 * `zone.run`, `zoneDelegate.invokeTask` for easier debugging, you can
 * import the following file, but please comment it out in production mode
 * because it will have performance impact when throw error
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
