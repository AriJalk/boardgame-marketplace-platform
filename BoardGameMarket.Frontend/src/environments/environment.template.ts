export const devEnvironment = {
  apiEndpoint: '[apiEndpoint],

  production: false,
  msalConfig: {
    auth: {
      clientId: '[SpaClientId]',
      authority: '[TenantAuthority]',
    },
  },
  apiConfig: {
    scopes: [Scopes],
    uri: '[ApiUri]',
  },
};
