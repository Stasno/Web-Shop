
import { API_URL } from "./ApiService";

export function ErrorNotification(err)
{
    console.warn(err+"");
    if (err.status === 401)
    {
        alert("Please log in");
        return;
    }
    else if (err.status === 400)
    {
        err.json().then(msg => 
            {   
                console.log("ErrorNotification", msg);
                if (msg.errorMsg)
                {
                    
                    alert("Error: " + msg.errorMsg);
                    return;
                }
                if (msg.errors)
                {
                    let errors = "";
                    for (let i in msg.errors)
                    {
                        errors += "["+i+"]"+ + "  " + msg.errors[i];
                    }
                    alert(errors);
                    return;
                }
                else
                    alert("Server side error");
            },
            err => alert("serverside error"));
            return;
    }
    alert("Api connection error");
    return;
}

class AuthService {

    stateCallback;

    CheckIsAuthenticated = () =>
    {
        fetch(API_URL + "account/IsSignIn")
            .then(res => res.ok ? res : Promise.reject(res))
            .then(res => 
                {
                    if (this.stateCallback)
                        this.stateCallback(true);
                })
            .catch(err => 
                {
                    localStorage.removeItem("user");
                    if (this.stateCallback)
                        this.stateCallback(false);
                })
    }

    IsAuthenticatedFast()
    {
        return (!!localStorage.key("user"));
    }

    SecuredResource()
    {
        if (this.IsAuthenticatedFast() === false)
        {
            window.location.href = "/login";
            this.stateCallback(false);
        }
    }

    login(data = true) {
        console.log("login");
        localStorage.setItem("user", data);
        this.stateCallback(true);
    }

    logout() {
        console.log("logout");
        localStorage.removeItem("user");
        fetch(API_URL + "account/logout");
        this.stateCallback(false);
        window.location.href = "/login";
    }


    getCurrentUser() {
        return JSON.parse(localStorage.getItem('user'));;
    }
}

export default new AuthService();