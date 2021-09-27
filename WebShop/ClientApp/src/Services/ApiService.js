import AuthService, { ErrorNotification } from "./AuthService";

export const API_URL = "https://localhost:44395/api/";

export function ApiFetchGet(setStateCallback, path, parameters = null)
{
    console.log("ApiFetch || " + API_URL + path + (parameters ? parameters : ""));
    fetch(API_URL + path + (parameters ? parameters : ""))
        .then(res => 
            {
                if (res.ok)
                {
                    return res.json();
                }
                else
                {
                    if (res.status === 401)
                    {
                        AuthService.logout();
                    }
                    return Promise.reject(res);
                }
            })
        .then(
            (res) => {
                setStateCallback({
                    isLoaded: true,
                    RequestResult: res,
                });
            }
        )
        .catch(err => 
            {
                ErrorNotification(err);
                console.log("error ApiFetch || " + API_URL + path, err);
                setStateCallback({
                    isLoaded: false,
                    error: err.code
                });
            });
}