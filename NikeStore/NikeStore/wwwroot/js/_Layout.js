setTimeout(function () {
    $(".Notification").fadeOut();
}, 3000);

window.addEventListener("scroll", function () {
    const header = document.querySelector("header");
    if (window.scrollY > 50) {
        header.classList.add("scrolled");
    } else {
        header.classList.remove("scrolled");
    }
});

document.addEventListener("DOMContentLoaded", () => {
    const chatbotInput = document.querySelector('.Chatbot_input');
    chatbotInput.style.opacity = '0';
    chatbotInput.style.pointerEvents = 'none';
    chatbotInput.style.transform = 'translateX(20%)';
});

function handleChatbot() {
    document.querySelector('.Chatbot_input').style.opacity = '1';
    document.querySelector('.Chatbot_input').style.pointerEvents = 'auto';
    document.querySelector('.Chatbot_input').style.transform = 'translateX(0)';
}

function closeChatbot() {
    document.querySelector('.Chatbot_input').style.opacity = '0';
    document.querySelector('.Chatbot_input').style.pointerEvents = 'none';
    document.querySelector('.Chatbot_input').style.transform = 'translateX(20%)';
}

function closeServices() {
    const closeServices = document.querySelector('.CloseServices');
    const openServices = document.querySelector('.OpenServices');
    closeServices.style.opacity = '0';
    closeServices.style.transform = 'translateX(-10%)';
    openServices.style.display = 'unset';
    closeServices.style.pointerEvents = 'none';
}

function openServices() {
    const closeServices = document.querySelector('.CloseServices');
    const openServices = document.querySelector('.OpenServices');
    openServices.style.display = 'none';
    closeServices.style.opacity = '1';
    closeServices.style.pointerEvents = 'auto';
    closeServices.style.transform = 'translateX(0)';
}

document.querySelectorAll('.menu-item').forEach(item => {
    const dropdownId = item.getAttribute('data-dropdown');
    const dropdown = document.getElementById(dropdownId);
    const background = document.querySelector('.dropdown-overlay');

    if (dropdown) {
        item.addEventListener('mouseenter', () => {
            dropdown.style.opacity = "1";
            dropdown.style.pointerEvents = "auto";
            background.style.pointerEvents = "auto";
            background.style.opacity = "1";
        });

        item.addEventListener('mouseleave', () => {
            dropdown.style.opacity = "0";
            dropdown.style.pointerEvents = "none";
            background.style.pointerEvents = "none";;
            background.style.opacity = "0";
        });

        dropdown.addEventListener('mouseenter', () => {
            dropdown.style.opacity = "1";
            dropdown.style.pointerEvents = "auto";
            background.style.pointerEvents = "auto";
            background.style.opacity = "1";
        });

        dropdown.addEventListener('mouseleave', () => {
            dropdown.style.opacity = "0";
            dropdown.style.pointerEvents = "none";
            background.style.pointerEvents = "none";
            background.style.opacity = "0";
        });
    }
});
