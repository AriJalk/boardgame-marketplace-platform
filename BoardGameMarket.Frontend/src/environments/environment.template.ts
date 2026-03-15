const apiAddress = '[API address]';

export const devEnvironment = {
  apiAddress: apiAddress,

  production: false,
  msalConfig: {
    auth: {
      clientId: '[Entra SPA ClientId]',
      authority: '[Entra Authority]',
    },
  },
  apiConfig: {
    accessAsUserScope: 'api://[clientId]/access_as_user',
    authEndpoints: [`${apiAddress}/reservations/`, `${apiAddress}/listing-sales/`],
    optionalAuthEndpoints: [`/api/listing-groups`, '/api/listings', '/api/users'],
  },
};
