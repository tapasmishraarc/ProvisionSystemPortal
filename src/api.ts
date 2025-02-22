import axios from 'axios';
import { ProvisioningRequest } from './types';

const API_BASE_URL = import.meta.env.VITE_AZURE_DEVOPS_API_URL;
const PAT_TOKEN = import.meta.env.VITE_AZURE_DEVOPS_PAT;

const api = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    Authorization: `Basic ${btoa(`:${PAT_TOKEN}`)}`,
    'Content-Type': 'application/json',
  },
});

export const triggerProvisioningPipeline = async (request: ProvisioningRequest) => {
  try {
    const response = await api.post('/pipelines/provision/runs', request);
    return response.data;
  } catch (error) {
    // Convert error to a simple string message
    const errorMessage = axios.isAxiosError(error)
      ? error.response?.data?.message || error.message
      : 'An unexpected error occurred';
    throw new Error(errorMessage);
  }
};