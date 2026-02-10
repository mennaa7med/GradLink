import api from './client';

// Get matches for a resume
export async function getMatchesByResume(resumeId) {
  const { data } = await api.get(`/api/matches/resume/${resumeId}`);
  return data;
}

// Get matches for a job
export async function getMatchesByJob(jobId) {
  const { data } = await api.get(`/api/matches/job/${jobId}`);
  return data;
}

// Run matching for a resume
export async function runMatching(resumeId) {
  const { data } = await api.post(`/api/matches/run/${resumeId}`);
  return data;
}

