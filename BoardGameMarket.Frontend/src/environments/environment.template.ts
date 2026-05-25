const API_BASE_URL = 'http://[API_ADDRESS]:[API_PORT]/api';
const API_APPLICATION_ID = '[API_APPLICATION_ID]';
const CLIENT_APPLICATION_ID = '[CLIENT_APPLICATION_ID]';
const MSAL_AUTHORITY = 'https://[TENANT_NAME].ciamlogin.com/';

export const environment = {
  apiAddress: API_BASE_URL,

  production: false,
  msalConfig: {
    auth: {
      clientId: CLIENT_APPLICATION_ID,
      authority: MSAL_AUTHORITY,
    },
  },
  apiConfig: {
    accessAsUserScope: `api://${API_APPLICATION_ID}/access_as_user`,
    authEndpoints: [
      `${API_BASE_URL}/reservations/`,
      `${API_BASE_URL}/listing-sales/`,
      `${API_BASE_URL}/listing-groups/owned/subscribers`,
      `${API_BASE_URL}/listing-groups/subscriptions`,
    ],
  },
};
