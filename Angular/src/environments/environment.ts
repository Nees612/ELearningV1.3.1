// This file can be replaced during build by using the `fileReplacements` array.
// `ng build --prod` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  API_USERS_URL: 'http://localhost:5000/Users',
  API_MODULES_URL: 'http://localhost:5000/Modules',
  API_ASSIGMENTS_URL: 'http://localhost:5000/Assigments',
  API_VIDEOS_URL: 'http://localhost:5000/Videos',
  API_MODULECONTENTS_URL: 'http://localhost:5000/ModuleContents',
  PROGRAMMING_BASICS: 'Programing Basics',
  WEB_TECHNOLOGIES: 'Web technologies',
  OOP: 'OOP',
  ADVANCED: 'Advanced',
  COOKIE_ID: 'tokenCookie'

};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/dist/zone-error';  // Included with Angular CLI.
