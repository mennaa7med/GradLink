import { api } from './client';

/**
 * Career API - Public endpoints for viewing all opportunities
 * No authentication required
 */

/**
 * Get all opportunities grouped by company
 * @param {Object} filter - Filter options
 * @param {string} filter.search - Search term
 * @param {string} filter.location - Location filter
 * @param {string} filter.type - Opportunity type: Job, Internship, Project
 * @param {string} filter.employmentType - Employment type: Full-time, Part-time, etc.
 * @param {string} filter.industry - Industry filter
 * @param {number} filter.page - Page number
 * @param {number} filter.pageSize - Items per page
 * @returns {Promise<Object>} Career page response with companies and opportunities
 */
export async function getAllOpportunities(filter = {}) {
  const params = new URLSearchParams();
  
  if (filter.search) params.append('search', filter.search);
  if (filter.location) params.append('location', filter.location);
  if (filter.type) params.append('type', filter.type);
  if (filter.employmentType) params.append('employmentType', filter.employmentType);
  if (filter.industry) params.append('industry', filter.industry);
  if (filter.page) params.append('page', filter.page);
  if (filter.pageSize) params.append('pageSize', filter.pageSize);
  
  const queryString = params.toString();
  const url = `/api/career${queryString ? `?${queryString}` : ''}`;
  
  const res = await api.get(url);
  return res.data;
}

/**
 * Get all jobs only
 * @param {string} search - Search term
 * @param {string} location - Location filter
 * @returns {Promise<Array>} List of job opportunities
 */
export async function getAllJobs(search = '', location = '') {
  const params = new URLSearchParams();
  if (search) params.append('search', search);
  if (location) params.append('location', location);
  
  const queryString = params.toString();
  const url = `/api/career/jobs${queryString ? `?${queryString}` : ''}`;
  
  const res = await api.get(url);
  return res.data;
}

/**
 * Get all internships only
 * @param {string} search - Search term
 * @returns {Promise<Array>} List of internship opportunities
 */
export async function getAllInternships(search = '') {
  const params = new URLSearchParams();
  if (search) params.append('search', search);
  
  const queryString = params.toString();
  const url = `/api/career/internships${queryString ? `?${queryString}` : ''}`;
  
  const res = await api.get(url);
  return res.data;
}

/**
 * Get all projects only
 * @param {string} search - Search term
 * @returns {Promise<Array>} List of project opportunities
 */
export async function getAllProjects(search = '') {
  const params = new URLSearchParams();
  if (search) params.append('search', search);
  
  const queryString = params.toString();
  const url = `/api/career/projects${queryString ? `?${queryString}` : ''}`;
  
  const res = await api.get(url);
  return res.data;
}

/**
 * Get opportunities for a specific company
 * @param {string} companyId - Company ID
 * @returns {Promise<Object>} Company with its opportunities
 */
export async function getCompanyOpportunities(companyId) {
  const res = await api.get(`/api/career/company/${companyId}`);
  return res.data;
}

/**
 * Get career page statistics
 * @returns {Promise<Object>} Statistics object
 */
export async function getCareerStats() {
  const res = await api.get('/api/career/stats');
  return res.data;
}


















