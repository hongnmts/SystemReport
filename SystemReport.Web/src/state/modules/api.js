import axios from "axios"

const path = process.env.VUE_APP_API_URL;


const get = (url, params) => {
  return axios.get(path + url, {params: params});
}

const postDelete = (url)=>{
  return axios.post(path + url); 
}

const post = async (url, value) =>{
  return axios({
    method: 'post',
    url: path + url,
    headers: {
      'Content-Type' : 'application/json'
    },
    data: JSON.stringify(value)
  })
}

export const apiClient = {get, post, postDelete }