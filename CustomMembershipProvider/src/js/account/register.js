import {Constants} from "../constants";
import {siteVerify} from "../services/recaptchaApi";

class Register {
    constructor() {
        this.signUpButton = document.getElementById(Constants.signUpButtonSelector());

        window.addEventListener('load', (event) => {
            
        });

        this.signUpButton.addEventListener('click', (event) => {
            event.preventDefault();

            grecaptcha.ready(function () {
                grecaptcha.execute(Constants.reCAPTCHASiteKey(), {action: 'signUp'}).then(function (token) {
                    return siteVerify(token, 'signUp')
                        .then(response => response.json())
                        .then(data => {
                            if (data.success) {
                                let score = parseFloat(data.score);
                                if (score >= 0.5)
                                {
                                    console.log(data);
                                }
                            }
                        });
                });
            });
        });
    }
}

export {Register};


