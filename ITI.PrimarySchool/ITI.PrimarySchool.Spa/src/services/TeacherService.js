const serviceUrl = process.env.VUE_APP_API_URL + '/api/teacher';

export const getById = async teacherId => {
  let response = await fetch(serviceUrl + '/' + teacherId);
  return await response.json();
}

export const getAll = async () => {
  let response = await fetch(serviceUrl);
  return await response.json();
}
