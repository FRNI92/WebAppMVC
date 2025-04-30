document.addEventListener('DOMContentLoaded', () => {
    if (!getCookie("Consent"))
        showCookieModal();
});

function showCookieModal() {
    const modal = document.getElementById('cookieModal');
    const card = document.querySelector('.card-cookie');
    if (modal) modal.classList.add("active");
    if (card) card.classList.add('active');

    const consentValue = getCookie('Consent')
    if (!consentValue) return
    try {
        const consent = JSON.parse(consentValue)
        document.getElementById("cookieDarkMode").checked = consent.darkmode
        document.getElementById("cookieFunctional").checked = consent.functional
        document.getElementById("cookieAnalytics").checked = consent.analytics
        document.getElementById("cookieMarketing").checked = consent.marketing
    }
    catch (error) {
        console.error('unable to handle cookie consent values', error)
    }
}

function hideCookieModal() {
    const modal = document.getElementById("cookieModal");
    const card = document.querySelector('.card-cookie');
    if (modal) modal.classList.remove("active");
    if (card) card.classList.remove('active');
}


function getCookie(name) {
    const nameEQ = name + "=";
    const cookies = document.cookie.split(';');
    for (let cookie of cookies) {
        cookie = cookie.trim();
        if (cookie.indexOf(nameEQ) === 0) {
            return decodeURIComponent(cookie.substring(nameEQ.length));
        }
    }
    return null;
}

function setCookie(name, value, days) {
    let expires = ""
    if (days) {
        const date = new Date()
        date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000)
        expires = "; expires=" + date.toUTCString()
    }

    const encodedValue = encodeURIComponent(value || "")
    document.cookie = `${name}=${encodedValue}${expires}; path=/; SameSite=Lax`
}

async function acceptAll() {
    const consent = {
        essential: true,
        darkmode: true,
        functional: true,
        analytics: true,
        marketing: true
    }

    setCookie("Consent", JSON.stringify(consent), 100)
    await handleConsent(consent)
    hideCookieModal()
}

async function acceptSelected() {
    const form = document.getElementById("cookieConsentForm");
    const formData = new FormData(form);

    const consent = {
        darkmode: formData.get("darkMode") === "on",
        essential: true,
        functional: formData.get("functional") === "on",
        analytics: formData.get("analytics") === "on",
        marketing: formData.get("marketing") === "on"
    };
    setCookie("Consent", JSON.stringify(consent), 100)
    await handleConsent(consent)
    hideCookieModal()

}
//async function setDarkModeCookie(isDark) {
//    await fetch('/cookies/set-darkmode', {
//        method: 'POST',
//        headers: { 'Content-Type': 'application/json' },
//        body: JSON.stringify(isDark)
//    });
//}

//this should go to a controller
async function handleConsent(consent) {
    try {
        const res = await fetch('/cookie-consent', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(consent)
        })

        if (!res.ok) {
            console.error('Unable to set cookie consent', await res.text())
        }
    }
    catch (error) {
        console.error("Error:: " , error)
    }
}


