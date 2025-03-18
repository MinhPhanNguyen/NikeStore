document.addEventListener("DOMContentLoaded", () => {
    const imgItems = document.querySelectorAll('.ImgItems img');
    const displayedImage = document.getElementById('displayedImage');
    const prevButton = document.querySelector('.PrevNext button:first-of-type');
    const nextButton = document.querySelector('.PrevNext button:last-of-type');
    let currentIndex = 0;

    if (imgItems.length > 0 && displayedImage) {
        imgItems.forEach((img, index) => {
            img.addEventListener('click', () => {
                currentIndex = index;
                displayedImage.src = img.src;
            });
        });

        function updateDisplayedImage(index) {
            if (index >= 0 && index < imgItems.length) {
                currentIndex = index;
                displayedImage.src = imgItems[currentIndex].src;
            }
        }

        if (prevButton && nextButton) {
            prevButton.addEventListener('click', () => {
                if (currentIndex > 0) {
                    currentIndex--;
                } else {
                    currentIndex = imgItems.length - 1;
                }
                updateDisplayedImage(currentIndex);
            });

            nextButton.addEventListener('click', () => {
                if (currentIndex < imgItems.length - 1) {
                    currentIndex++;
                } else {
                    currentIndex = 0;
                }
                updateDisplayedImage(currentIndex);
            });
        }
    } else {
        console.error("Không tìm thấy hình ảnh hoặc phần tử cần thiết!");
    }
});

function previewImages(event) {
    var input = event.target;
    var container = document.getElementById('imagePreviewContainer');

    if (!input.files || input.files.length === 0) {
        container.innerHTML = "<p style='color:red;'>Không có ảnh nào được chọn.</p>";
        return;
    }

    container.innerHTML = '';

    for (let i = 0; i < input.files.length; i++) {
        let file = input.files[i];
        let reader = new FileReader();

        reader.onload = function (e) {
            let img = document.createElement('img');
            img.src = e.target.result;
            img.style.width = "100px";
            img.style.height = "100px";
            img.style.objectFit = "cover";
            img.style.border = "1px solid #ddd";
            img.style.borderRadius = "8px";

            container.appendChild(img);
        };

        reader.readAsDataURL(file);
    }
}

setTimeout(function () {
    $(".Notification").fadeOut();
}, 2000);

setTimeout(function () {
    $(".notification").fadeOut();
}, 2000);

function toggleMenu(element) {
    const allMenus = document.querySelectorAll(".Title .Contain");
    const sidebarMenu = document.querySelector(" .Sidebar");

    if (sidebarMenu.style.width === "5%") {
        return;
    }

    allMenus.forEach(menu => {
        menu.style.maxHeight = "0";
        menu.style.opacity = "0";
        menu.style.pointerEvents = "none";
    });

    const selectedMenu = element.parentElement.querySelector(".Contain");
    const currentHeight = window.getComputedStyle(selectedMenu).maxHeight;

    if (currentHeight !== "0px") {
        selectedMenu.style.maxHeight = "0";
        selectedMenu.style.opacity = "0";
        selectedMenu.style.pointerEvents = "none";
    } else {
        selectedMenu.style.maxHeight = "100vh";
        selectedMenu.style.opacity = "1";
        selectedMenu.style.pointerEvents = "auto";
    }
}

function toggleChildMenu(element) {
    const allChildMenus = document.querySelectorAll(".MenuContent ul");
    const sidebarMenu = document.querySelector(" .Sidebar");

    if (sidebarMenu.style.width === "5%") {
        return;
    }

    allChildMenus.forEach(menu => {
        menu.style.display = "none";
    });

    const childMenu = element.parentElement.querySelector("ul");
    const currentDisplay = window.getComputedStyle(childMenu).display;

    if (currentDisplay !== "none") {
        childMenu.style.display = "none";
    } else {
        childMenu.style.display = "block";
    }
}

window.addEventListener("click", function (e) {
    const menuOption = document.querySelector(" .MenuOption");
    const menuList = document.querySelector(" .Sidebar");
    const logoIMG = document.querySelector(" .Sidebar .Logo img");
    const logo = document.querySelector(" .Sidebar .Logo");
    const contain = document.querySelectorAll(" .Sidebar .Title .MenuContent");
    const textElements = document.querySelectorAll(" .Sidebar .Title label");
    const paddingH2 = document.querySelectorAll(" .Sidebar .Title h2");
    const content = document.querySelector(".DashboardContent");
    const span = document.querySelectorAll(" .Sidebar .Title h2 span");

    if (e.target.closest(".MenuOption")) {
        if (menuList.style.width === "20%" || menuList.style.width === "") {
            menuList.style.width = "5%";
            menuOption.style.left = "5%";
            logoIMG.style.height = "2vh";
            logo.style.marginBottom = "0";
            logoIMG.style.marginLeft = "0.8em";
            content.style.right = "50%";
            content.style.transform = "translateX(50%)";

            span.forEach((span) => {
                span.style.position = "absolute";
                span.style.transform = "translate(-0.2em,-0.4em)";
            });

            contain.forEach((title) => {
                title.style.display = "none";
            });

            textElements.forEach((text) => {
                text.style.opacity = "0";
                text.style.pointerEvents = "none";
            });

            paddingH2.forEach((h2) => {
                h2.style.padding = "1.2em 1.5em"
            });
        } else {
            menuList.style.width = "20%";
            menuOption.style.left = "20%";
            logoIMG.style.height = "4vh";
            logo.style.marginBottom = "0.5em";
            logoIMG.style.marginLeft = "1.8em";
            content.style.right = "0";
            content.style.transform = "translateX(0)";

            span.forEach((span) => {
                span.style.position = "relative";
                span.style.transform = "translate(0,0)";
            });

            contain.forEach((title) => {
                title.style.display = "block";
            });

            textElements.forEach((text) => {
                text.style.opacity = "1";
                text.style.pointerEvents = "auto";
            });

            paddingH2.forEach((h2) => {
                h2.style.padding = "1em 2em"
            });
        }
    }
});

window.addEventListener("DOMContentLoaded", function () {
    const toggleSections = document.querySelectorAll(".OnOffSection");
    const toggleSectionsUpDown = document.querySelectorAll(".UpDownOption");
    const toggleSectionsHideShow = document.querySelectorAll(".ShowHideOption");

    toggleSections.forEach(section => {
        section.addEventListener("click", function () {
            toggleOnOff(this);
        });
    });

    toggleSectionsUpDown.forEach(section => {
        section.addEventListener("click", function () {
            toggleUpDown(this);
        });
    });

    toggleSectionsHideShow.forEach(section => {
        section.addEventListener("click", function () {
            toggleHideShow(this);
        });
    });

    showSetting('GeneralSetting', this);
});

function toggleDateInput() {
    const option = document.getElementById("filterType").value;
    const date = document.getElementById("dateInput");
    const month = document.getElementById("monthInput");
    const year = document.getElementById("yearInput");

    date.style.display = "none";
    month.style.display = "none";
    year.style.display = "none";

    if (option === "day") {
        date.style.display = "unset";
    } else if (option === "month") {
        month.style.display = "unset";
    } else if (option === "year") {
        year.style.display = "unset";
    }
}

function showSetting(page, element) {
    let pages = document.querySelectorAll('.SettingContent > div');
    let titles = document.querySelectorAll('.DashboardContent .DashboardSetting .SettingSidebar ul li');
    pages.forEach(page => {
        page.style.display = "none";
    });

    titles.forEach(title => {
        title.style.color = "#adadad";
    });

    document.getElementById(page).style.display = "flex";
    element.style.color = "#2e2e2e";
};

function toggleListOption(element) {
    const parent = element.closest(".Time").parentElement;
    const listMes = parent.querySelector(".ListOption");

    const isHidden = getComputedStyle(listMes).opacity === "0";

    if (isHidden) {
        listMes.style.opacity = "1";
        listMes.style.pointerEvents = "auto";
        listMes.style.maxHeight = "30vh";
    } else {
        listMes.style.opacity = "0";
        listMes.style.pointerEvents = "none";
        listMes.style.maxHeight = "0";
    }
}

function toggleMenuItems() {
    const menu = document.querySelector(".DashboardContent .DashboardMessaging .MessagingContent .MainMesaging .Items");

    if (menu.style.opacity === "0" || menu.style.opacity === "") {
        menu.style.opacity = "1"
        menu.style.maxWidth = "30%";
        menu.style.pointerEvents = "auto";
    } else {
        menu.style.opacity = "0"
        menu.style.maxWidth = "0";
        menu.style.pointerEvents = "none";
    }
}

function toggleInputItems() {
    const menu = document.querySelector(".DashboardContent .DashboardMessaging .MessagingContent .MainMesaging .MainTitle .OtherLink");

    if (menu.style.opacity === "0" || menu.style.opacity === "") {
        menu.style.opacity = "1"
        menu.style.pointerEvents = "auto";
        menu.style.right = "5.5em";
    } else {
        menu.style.opacity = "0"
        menu.style.pointerEvents = "none";
        menu.style.right = "7em";
    }
}

function toggleOnOff(section) {
    const toggleOn = section.querySelector(".ToggleOn");
    const toggleOff = section.querySelector(".ToggleOff");

    if (getComputedStyle(toggleOn).display === "none") {
        toggleOn.style.display = "flex";
        toggleOff.style.display = "none";
    } else {
        toggleOn.style.display = "none";
        toggleOff.style.display = "flex";
    }
}

function toggleUpDown(section) {
    const UpSection = section.querySelector(".UpSection");
    const DownSection = section.querySelector(".DownSection");
    const content = section.nextElementSibling;

    if (getComputedStyle(UpSection).display === "none") {
        UpSection.style.display = "flex";
        DownSection.style.display = "none";
        content.style.opacity = "1";
        content.style.maxHeight = " 100vh";
        content.style.pointerEvents = "auto";
        content.style.marginBottom = "1em";
    } else {
        UpSection.style.display = "none";
        DownSection.style.display = "flex";
        content.style.opacity = "0";
        content.style.maxHeight = "0";
        content.style.pointerEvents = "none";
        content.style.marginBottom = "0";
    }
}

function toggleHideShow(section) {
    const showIcon = section.querySelector(".Show");
    const hideIcon = section.querySelector(".Hide");
    const inputPass = section.querySelector(".ShowHideOption input");

    if (getComputedStyle(showIcon).display === "none") {
        inputPass.type = "password";
        showIcon.style.display = "flex";
        hideIcon.style.display = "none";
    } else {
        inputPass.type = "text";
        showIcon.style.display = "none";
        hideIcon.style.display = "flex";
    }
};

function toggleFilter(section) {
    const showFilter = section.querySelector(".Show");
    const hideFilter = section.querySelector(".Hide");
    const content = section.querySelector(".FilterManager")

    const isHidden = getComputedStyle(content).opacity === "0";

    if (isHidden) {
        hideFilter.style.display = "flex";
        showFilter.style.display = "none";
        content.style.opacity = "1";
        content.style.pointerEvents = "auto";
        content.style.minWidth = "30%";
        content.style.maxWidth = "50%";
        content.style.zIndex = "10";
    } else {
        hideFilter.style.display = "none";
        showFilter.style.display = "flex";
        content.style.opacity = "0";
        content.style.pointerEvents = "none";
        content.style.minWidth = "0%";
        content.style.maxWidth = "0%";
        content.style.zIndex = "0";
    }
};

function updateFileName() {
    const fileInput = document.getElementById("file-upload");
    const fileName = fileInput.files[0] ? fileInput.files[0].name : "Chưa chọn tệp nào";

    document.getElementById("file-name").textContent = fileName;
}