export interface System {
  id: string;
  name: string;
}

export interface Version {
  id: string;
  version: string;
}

export interface ProvisioningRequest {
  systems: string[];
  version: string;
  action: 'provision' | 'deprovision';
}