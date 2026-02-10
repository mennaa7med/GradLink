import api from './client';

// ============ MATERIALS ============

export const getMaterials = async (filters = {}) => {
  const params = new URLSearchParams();
  
  if (filters.type) params.append('Type', filters.type);
  if (filters.field && filters.field !== 'all') params.append('Field', filters.field);
  if (filters.searchTerm) params.append('SearchTerm', filters.searchTerm);
  if (filters.status) params.append('Status', filters.status);
  if (filters.difficultyLevel) params.append('DifficultyLevel', filters.difficultyLevel);
  if (filters.sortBy) params.append('SortBy', filters.sortBy);
  if (filters.isFeatured !== undefined) params.append('IsFeatured', filters.isFeatured);
  if (filters.page) params.append('Page', filters.page);
  if (filters.pageSize) params.append('PageSize', filters.pageSize);
  
  const { data } = await api.get(`/api/materials?${params.toString()}`);
  return data;
};

export const getMaterial = async (id) => {
  const { data } = await api.get(`/api/materials/${id}`);
  return data;
};

export const createMaterial = async (materialData) => {
  const { data } = await api.post('/api/materials', materialData);
  return data;
};

export const updateMaterial = async (id, materialData) => {
  const { data } = await api.put(`/api/materials/${id}`, materialData);
  return data;
};

export const deleteMaterial = async (id) => {
  const { data } = await api.delete(`/api/materials/${id}`);
  return data;
};

export const reviewMaterial = async (id, reviewData) => {
  const { data } = await api.post(`/api/materials/${id}/review`, reviewData);
  return data;
};

export const getPendingMaterials = async () => {
  const { data } = await api.get('/api/materials/pending');
  return data;
};

export const getMaterialStats = async () => {
  const { data } = await api.get('/api/materials/stats');
  return data;
};

export const getMyMaterialStats = async () => {
  const { data } = await api.get('/api/materials/my-stats');
  return data;
};

export const getRecommendations = async () => {
  const { data } = await api.get('/api/materials/recommendations');
  return data;
};

export const getTopRated = async (count = 10) => {
  const { data } = await api.get(`/api/materials/top-rated?count=${count}`);
  return data;
};

export const getFeatured = async () => {
  const { data } = await api.get('/api/materials/featured');
  return data;
};

// ============ RATINGS ============

export const getMaterialRatings = async (materialId) => {
  const { data } = await api.get(`/api/materials/${materialId}/ratings`);
  return data;
};

export const addRating = async (materialId, ratingData) => {
  const { data } = await api.post(`/api/materials/${materialId}/ratings`, ratingData);
  return data;
};

export const deleteRating = async (materialId, ratingId) => {
  const { data } = await api.delete(`/api/materials/${materialId}/ratings/${ratingId}`);
  return data;
};

export const markRatingHelpful = async (materialId, ratingId) => {
  const { data } = await api.post(`/api/materials/${materialId}/ratings/${ratingId}/helpful`);
  return data;
};

// ============ PROGRESS ============

export const getMaterialProgress = async (materialId) => {
  const { data } = await api.get(`/api/materials/${materialId}/progress`);
  return data;
};

export const updateMaterialProgress = async (materialId, progressData) => {
  const { data } = await api.put(`/api/materials/${materialId}/progress`, progressData);
  return data;
};

export const toggleBookmark = async (materialId) => {
  const { data } = await api.post(`/api/materials/${materialId}/bookmark`);
  return data;
};

export const getBookmarkedMaterials = async () => {
  const { data } = await api.get('/api/materials/bookmarked');
  return data;
};

export const getInProgressMaterials = async () => {
  const { data } = await api.get('/api/materials/in-progress');
  return data;
};

export const getCompletedMaterials = async () => {
  const { data } = await api.get('/api/materials/completed');
  return data;
};

export const getRecentlyViewedMaterials = async (count = 10) => {
  const { data } = await api.get(`/api/materials/recently-viewed?count=${count}`);
  return data;
};

// ============ NOTES ============

export const getMaterialNotes = async (materialId) => {
  const { data } = await api.get(`/api/materials/${materialId}/notes`);
  return data;
};

export const addNote = async (materialId, noteData) => {
  const { data } = await api.post(`/api/materials/${materialId}/notes`, noteData);
  return data;
};

export const updateNote = async (materialId, noteId, noteData) => {
  const { data } = await api.put(`/api/materials/${materialId}/notes/${noteId}`, noteData);
  return data;
};

export const deleteNote = async (materialId, noteId) => {
  const { data } = await api.delete(`/api/materials/${materialId}/notes/${noteId}`);
  return data;
};

export const getAllMyNotes = async () => {
  const { data } = await api.get('/api/materials/my-notes');
  return data;
};

// ============ COLLECTIONS ============

export const getCollections = async (type, field) => {
  const params = new URLSearchParams();
  if (type) params.append('type', type);
  if (field) params.append('field', field);
  const { data } = await api.get(`/api/materialcollections?${params.toString()}`);
  return data;
};

export const getCollection = async (id) => {
  const { data } = await api.get(`/api/materialcollections/${id}`);
  return data;
};

export const createCollection = async (collectionData) => {
  const { data } = await api.post('/api/materialcollections', collectionData);
  return data;
};

export const updateCollection = async (id, collectionData) => {
  const { data } = await api.put(`/api/materialcollections/${id}`, collectionData);
  return data;
};

export const deleteCollection = async (id) => {
  const { data } = await api.delete(`/api/materialcollections/${id}`);
  return data;
};

export const addMaterialToCollection = async (collectionId, materialData) => {
  const { data } = await api.post(`/api/materialcollections/${collectionId}/materials`, materialData);
  return data;
};

export const removeMaterialFromCollection = async (collectionId, materialId) => {
  const { data } = await api.delete(`/api/materialcollections/${collectionId}/materials/${materialId}`);
  return data;
};

export const reorderCollectionMaterials = async (collectionId, materialIds) => {
  const { data } = await api.put(`/api/materialcollections/${collectionId}/materials/reorder`, materialIds);
  return data;
};

export const followCollection = async (collectionId) => {
  const { data } = await api.post(`/api/materialcollections/${collectionId}/follow`);
  return data;
};

export const unfollowCollection = async (collectionId) => {
  const { data } = await api.delete(`/api/materialcollections/${collectionId}/follow`);
  return data;
};

export const getMyCollections = async () => {
  const { data } = await api.get('/api/materialcollections/my-collections');
  return data;
};

export const getFollowingCollections = async () => {
  const { data } = await api.get('/api/materialcollections/following');
  return data;
};

export const getLearningPaths = async (field) => {
  const params = field ? `?field=${field}` : '';
  const { data } = await api.get(`/api/materialcollections/learning-paths${params}`);
  return data;
};
