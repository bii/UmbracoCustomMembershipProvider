let siteVerify = async (response, action) => {
    return await fetch(`/umbraco/api/recaptcha/siteverify?recaptchaResponse=${response}&recaptchaAction=${action}`, {
        headers: {"Content-Type": "application/json; charset=utf-8"},
        method: 'POST'
    })
};

export {siteVerify};