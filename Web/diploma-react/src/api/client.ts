import { authStore } from "../App";

const handleResponse = async (response: Response) => {
  if (!response.ok) {
    const message = await response.json();
    throw Error(message.error || "Request error");
  }

  console.log(response);
  return response.json();
};

let headers: { "Content-Type": string; Authorization?: string; } | { "Content-Type": string; };

// if (authStore?.user) {
//   headers = { "Content-Type": "application/json", 'Authorization': 'Bearer ' + authStore.user.access_token };
// }
// else {
//   headers = { "Content-Type": "application/json" }
// }

const apiClient = async ({ url, path, method, data }: apiClientProps) => {
  const requestOptions = {
    method,
    headers: { "Content-Type": "application/json", 'Authorization': 'Bearer ' + authStore?.user?.access_token },
    body: !!data ? JSON.stringify(data) : undefined,
  };
  return await fetch(`${url}${path}`, requestOptions).then(handleResponse);
};

interface apiClientProps {
  url: string
  path: string;
  method: string;
  data?: unknown;
}

export default apiClient;
