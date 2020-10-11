import { config } from "dotenv";
export const getBaseUrl = () => {
  config();
  const staging = process.env.STAGING as string;
  if (staging == "localhost")
    return process.env.AZ_LOCALHOST_BASE_URL as string;
  else return process.env.AZ_REMOTE_BASE_URL as string;
};

export const postJson = async (data: any, url: string) => {
  const response = await fetch(url, {
    method: "POST",
    mode: "cors",
    cache: "no-cache",
    credentials: "same-origin",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(data),
  });
  return response.json();
};
