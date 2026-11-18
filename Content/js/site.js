/* FoodBridge Client-Side Interactivity & Print Helpers */
function toggleMobileMenu() {
    var nav = document.getElementById('mainNavLinks');
    if (nav) {
        if (nav.className.indexOf('show') !== -1) {
            nav.className = nav.className.replace(' show', '');
        } else {
            nav.className += ' show';
        }
    }
}

function printReportArea() {
    window.print();
    return false;
}

function fillDemoCredentials(email, password, role) {
    var emailBox = document.getElementById('MainContent_txtEmail') || document.querySelector('input[id*="txtEmail"]');
    var passBox = document.getElementById('MainContent_txtPassword') || document.querySelector('input[id*="txtPassword"]');
    var roleSelect = document.getElementById('MainContent_ddlRole') || document.querySelector('select[id*="ddlRole"]');
    if (emailBox) emailBox.value = email;
    if (passBox) passBox.value = password;
    if (roleSelect && role) roleSelect.value = role;
    return false;
}
