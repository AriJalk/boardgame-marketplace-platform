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
    accessAsUserScope: 'api://[Entra API client Id]/access_as_user',
    authEndpoints: [
      `${apiAddress}/reservations/`,
      `${apiAddress}/listing-sales/`,
      `${apiAddress}/listing-groups/owned/subscribers`,
      `${apiAddress}/listing-groups/subscriptions`,
    ],
    // optionalAuthEndpoints: [`/api/listing-groups`, '/api/listings', '/api/users'],
  },
};
