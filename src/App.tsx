import React, { useState } from 'react';
import Select from 'react-select';
import { Settings } from 'lucide-react';
import { triggerProvisioningPipeline } from './api';
import { System, Version } from './types';

const systems: System[] = [
  { id: '1', name: 'System1' },
  { id: '2', name: 'System2' },
  { id: '3', name: 'System3' },
];

const versions: Version[] = [
  { id: '1', version: 'v1.0.0' },
];

function App() {
  const [selectedSystems, setSelectedSystems] = useState<System[]>([]);
  const [selectedVersion, setSelectedVersion] = useState<Version | null>(null);
  const [action, setAction] = useState<'provision' | 'deprovision'>('provision');
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!selectedVersion || selectedSystems.length === 0) return;

    setIsLoading(true);
    setError(null);

    try {
      await triggerProvisioningPipeline({
        systems: selectedSystems.map(s => s.id),
        version: selectedVersion.version,
        action,
      });
      alert(`${action === 'provision' ? 'Provisioning' : 'Deprovisioning'} pipeline triggered successfully!`);
    } catch (error) {
      const errorMessage = error instanceof Error ? error.message : 'An unexpected error occurred';
      setError(errorMessage);
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <header className="bg-white shadow">
        <div className="max-w-7xl mx-auto px-4 py-6">
          <div className="flex items-center">
            <Settings className="w-8 h-8 text-blue-600 mr-3" />
            <h1 className="text-3xl font-bold text-gray-900">System Provisioning</h1>
          </div>
        </div>
      </header>

      <main className="max-w-7xl mx-auto px-4 py-8">
        <form onSubmit={handleSubmit} className="bg-white shadow rounded-lg p-6">
          {error && (
            <div className="mb-4 p-4 text-red-700 bg-red-100 rounded-md border border-red-300">
              {error}
            </div>
          )}
          
          <div className="space-y-6">
            <div>
              <h2 className="text-xl font-semibold mb-2">Select Systems</h2>
              <p className="text-sm text-gray-600 mb-2">Choose one or more systems:</p>
              <Select
                isMulti
                options={systems}
                getOptionLabel={(option) => option.name}
                getOptionValue={(option) => option.id}
                value={selectedSystems}
                onChange={(selected) => setSelectedSystems(selected as System[])}
                className="w-full"
              />
            </div>

            <div>
              <h2 className="text-xl font-semibold mb-2">Select Version</h2>
              <p className="text-sm text-gray-600 mb-2">Choose a version:</p>
              <Select
                options={versions}
                getOptionLabel={(option) => option.version}
                getOptionValue={(option) => option.id}
                value={selectedVersion}
                onChange={(selected) => setSelectedVersion(selected as Version)}
                className="w-full"
              />
            </div>

            <div>
              <h2 className="text-xl font-semibold mb-2">Action</h2>
              <div className="flex space-x-4">
                <label className="flex items-center">
                  <input
                    type="radio"
                    value="provision"
                    checked={action === 'provision'}
                    onChange={(e) => setAction(e.target.value as 'provision')}
                    className="mr-2"
                  />
                  Provision
                </label>
                <label className="flex items-center">
                  <input
                    type="radio"
                    value="deprovision"
                    checked={action === 'deprovision'}
                    onChange={(e) => setAction(e.target.value as 'deprovision')}
                    className="mr-2"
                  />
                  Deprovision
                </label>
              </div>
            </div>

            <button
              type="submit"
              disabled={isLoading || !selectedVersion || selectedSystems.length === 0}
              className={`w-full py-2 px-4 rounded-md text-white font-medium ${
                isLoading || !selectedVersion || selectedSystems.length === 0
                  ? 'bg-gray-400 cursor-not-allowed'
                  : 'bg-blue-600 hover:bg-blue-700'
              }`}
            >
              {isLoading ? 'Processing...' : 'Submit'}
            </button>
          </div>
        </form>

        <footer className="mt-8 text-center text-gray-600">
          © 2025 - System Provisioning Portal
        </footer>
      </main>
    </div>
  );
}

export default App;