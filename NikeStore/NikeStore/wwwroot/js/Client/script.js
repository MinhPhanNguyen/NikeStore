setTimeout(function () {
    $(".Notification").fadeOut();
}, 3000);

function toggleShipping() {
    const content = document.querySelector(".ContentShipping");
    const isHidden = getComputedStyle(content).display === "none";

    if (isHidden) {
        content.style.setProperty("display", "flex", "important");
    } else {
        content.style.setProperty("display", "none", "important"); 
    }
}

function toggleMenuAccount() {
    const open = document.querySelector('.MenuAccount');
    const close = document.querySelector('.AccountClose');
    const content = document.querySelector('.AccountPage .Container .SideBar')

    const isHidden = getComputedStyle(content).display === "none";

    if (isHidden) {
        open.style.display = "none";
        close.style.display = "flex";
        content.style.display = "flex"
    } else {
        open.style.display = "flex";
        close.style.display = "none";
        content.style.display = "none"
    }
}

function showFilter() {
    const parent = document.querySelector('.Filter'); 
    var category = document.querySelector('.ProductListPage .Section_2 .Category');
    var productList = document.querySelector('.ProductListPage .Section_2 .ProductList');
    parent.querySelector('.Show').style.display = 'none';
    parent.querySelector('.Hide').style.display = 'block';
    category.style.opacity = '1';
    category.style.pointerEvents = 'auto';
    productList.style.width = '80%';
    productList.style.marginLeft = '20%';
};

function hideFilter() {
    const parent = document.querySelector('.Filter');
    var category = document.querySelector('.ProductListPage .Section_2 .Category');
    var productList = document.querySelector('.ProductListPage .Section_2 .ProductList');
    parent.querySelector('.Show').style.display = 'block';
    parent.querySelector('.Hide').style.display = 'none';
    category.style.opacity = '0';
    category.style.pointerEvents = 'none';
    productList.style.width = '100%';
    productList.style.marginLeft = '0';
};

function toggleOption() {
    const dropdown = document.querySelector(".ProductListPage .Section_1 .dropdown");
    const upIcon = document.querySelector(".UpSort");
    const downIcon = document.querySelector(".DownSort");

    if (dropdown.style.opacity === "0") {
        dropdown.style.maxHeight = '50vh'
        dropdown.style.opacity = "1";
        upIcon.style.display = "block";
        downIcon.style.display = "none";
    } else {
        dropdown.style.maxHeight = '0'
        dropdown.style.opacity = "0";
        upIcon.style.display = "none";
        downIcon.style.display = "block";
    }
}

function toggleFilter(element) {
    const filterItem = element.closest(".FilterItems"); 
    const option = filterItem.querySelector(".Option");
    const upIcon = filterItem.querySelector(".UpFilter");
    const downIcon = filterItem.querySelector(".DownFilter");

    if (option.style.opacity === "0") {
        option.style.maxHeight = "50vh";
        option.style.opacity = "1";
        upIcon.style.display = "block";
        downIcon.style.display = "none";
    } else {
        option.style.maxHeight = "0";
        option.style.opacity = "0";
        upIcon.style.display = "none";
        downIcon.style.display = "block";
    }
}

function toggleFilterColour(element) {
    const filterItem = element.closest(".FilterItems");
    const option = filterItem.querySelector(".OptionColour");
    const upIcon = filterItem.querySelector(".UpFilter");
    const downIcon = filterItem.querySelector(".DownFilter");

    if (option.style.opacity === "0") {
        option.style.maxHeight = "30vh";
        option.style.opacity = "1";
        upIcon.style.display = "block";
        downIcon.style.display = "none";
    } else {
        option.style.maxHeight = "0";
        option.style.opacity = "0";
        upIcon.style.display = "none";
        downIcon.style.display = "block";
    }
}

document.addEventListener('DOMContentLoaded', function () {
    handleScroll();
});

window.addEventListener('scroll', handleScroll);
window.addEventListener('resize', handleScroll);
window.addEventListener('load', handleScroll);

function handleScroll() {
    var category = document.querySelector('.ProductListPage .Section_2 .Category');
    var section = document.querySelector('.ProductListPage .Section_2');
    var productList = document.querySelector('.ProductListPage .Section_2 .ProductList');

    if (!category || !section || !productList) return;

    var sectionTop = section.getBoundingClientRect().top;
    var sectionBottom = productList.getBoundingClientRect().bottom;

    if (sectionTop <= 0 && sectionBottom > window.innerHeight) {
        category.style.position = 'fixed';
        category.style.top = '12vh';
        category.style.zIndex = '999';
        category.style.height = '80vh';
        category.classList.add('CategoryTop');
    } else if (sectionBottom <= window.innerHeight) {
        category.style.position = 'absolute';
        category.style.top = (productList.offsetHeight - category.offsetHeight) + 'px';
        category.style.height = '80vh';
        category.style.zIndex = '10';
    } else {
        category.style.position = 'absolute';
        category.style.height = '70vh';
        category.style.zIndex = '10';
        category.style.top = '0';
    }
}

window.addEventListener("scroll", function () {
    var filterOption = document.querySelector('.ProductListPage .Section_1 .Filter');
    var dropdown = document.querySelector(' .ProductListPage .Section_1 .dropdown');
    if (!filterOption) return;

    if (window.scrollY > 50) {
        filterOption.classList.add('scrollFilter'); 
        dropdown.classList.add('scrollFilter'); 
    } else {
        filterOption.classList.remove('scrollFilter');
        dropdown.classList.remove('scrollFilter');
    }
});

function toggleDescription(element) {
    const option = document.querySelector(".ProductDetail .Section_2 .Container .Content");
    const upIcon = document.querySelector(".UpDes");
    const downIcon = document.querySelector(".DownDes");

    if (option.style.opacity === "0") {
        option.style.maxHeight = "300vh";
        option.style.marginBottom = "3em";
        option.style.opacity = "1";
        upIcon.style.display = "block";
        downIcon.style.display = "none";
    } else {
        option.style.maxHeight = "0";
        option.style.opacity = "0";
        option.style.marginBottom = "0";
        upIcon.style.display = "none";
        downIcon.style.display = "block";
    }
}

function toggleRate(element) {
    const option = document.querySelector(".ProductDetail .Section_3 .RateContent");
    const upIcon = document.querySelector(".UpRate");
    const downIcon = document.querySelector(".DownRate");

    if (option.style.opacity === "0") {
        option.style.maxHeight = "300vh";
        option.style.opacity = "1";
        upIcon.style.display = "block";
        downIcon.style.display = "none";
    } else {
        option.style.maxHeight = "0";
        option.style.opacity = "0";
        upIcon.style.display = "none";
        downIcon.style.display = "block";
    }
}

function toggleWaranty(element) {
    const content = element.nextElementSibling;
    const title = element;
    const addContent = element.querySelector('.ServicePage .Container .Section_2 ul li .Title .AddService');
    const removeContent = element.querySelector('.ServicePage .Container .Section_2 ul li .Title .RemoveService');

    if (content.style.opacity === "0") {
        content.style.maxHeight = "300vh";
        content.style.opacity = "1";
        content.style.pointerEvents = "auto";
        removeContent.style.display = "block";
        addContent.style.display = "none";
        title.style.color = "black";
    } else {
        content.style.maxHeight = "0";
        content.style.opacity = "0";
        content.style.pointerEvents = "none";
        removeContent.style.display = "none";
        addContent.style.display = "block";
        title.style.color = "#c3c3c3";
    }
}

document.addEventListener("DOMContentLoaded", () => {
    const imgItems = document.querySelectorAll('.ProductDetail .Section_1 .Contain_img .ImgItems img');
    const displayedImage = document.getElementById('displayedImage');
    const prevButton = document.querySelector('.PrevNext button:first-of-type');
    const nextButton = document.querySelector('.PrevNext button:last-of-type');
    let currentIndex = 0;

    if (imgItems.length > 0 && displayedImage) {
        imgItems.forEach(img => {
            img.addEventListener('click', () => {
                displayedImage.src = img.src;
            });
        });

        function updateDisplayedImage(index) {
            currentIndex = index;
            displayedImage.src = imgItems[currentIndex].src;
        }

        if (prevButton && nextButton) {
            prevButton.addEventListener('click', () => {
                currentIndex = (currentIndex - 1 + imgItems.length) % imgItems.length;
                updateDisplayedImage(currentIndex);
            });

            nextButton.addEventListener('click', () => {
                currentIndex = (currentIndex + 1) % imgItems.length;
                updateDisplayedImage(currentIndex);
            });
        }
    }

    // Xử lý tăng giảm số lượng trong giỏ hàng
    document.querySelectorAll(".ItemsFeature").forEach((feature) => {
        const span = feature.querySelector(".btn span");
        const minus = feature.querySelector(".btn .Minus");
        const plus = feature.querySelector(".btn .Plus");
        const trash = feature.querySelector(".btn .Trash");

        if (span && minus && plus && trash) {
            const updateIcons = () => {
                const value = parseInt(span.textContent);

                if (value > 1) {
                    minus.style.display = 'unset';
                    trash.style.display = 'none';
                } else {
                    minus.style.display = 'none';
                    trash.style.display = 'unset';
                }
            };

            plus.addEventListener("click", () => {
                let value = parseInt(span.textContent);
                span.textContent = value + 1;
                updateIcons();
            });

            minus.addEventListener("click", () => {
                let value = parseInt(span.textContent);
                if (value > 1) {
                    span.textContent = value - 1;
                    updateIcons();
                }
            });

            updateIcons(); // Cập nhật ban đầu
        }
    });

    // Xử lý thêm vào danh sách yêu thích
    document.querySelectorAll('.btnFavour').forEach(favour => {
        favour.addEventListener("click", () => {
            const heartIcon = favour.querySelector('i');
            const currentOpacity = parseFloat(window.getComputedStyle(heartIcon).opacity);

            if (currentOpacity === 0.8) {
                heartIcon.style.color = "white";
                heartIcon.style.border = "1px solid #ff5637";
                heartIcon.style.backgroundColor = "#ff5637";
                heartIcon.style.opacity = "1";
            } else {
                heartIcon.style.color = "#d5d5d5";
                heartIcon.style.border = "1px solid #d5d5d5";
                heartIcon.style.backgroundColor = "transparent";
                heartIcon.style.opacity = "0.8";
            }
        });
    });

    showPage('AccountHome');
});

function toggleItems(element) {
    const items = element.parentElement;
    const content = items.querySelector('.ContentItems');
    const title = items.querySelector('.TitleItems');
    const upItem = items.querySelector('.UpItems');
    const downItem = items.querySelector('.DownItems');

    const isHidden = content.style.opacity === "0" || content.style.opacity === "";

    if (isHidden) {
        content.style.opacity = "1";
        content.style.maxHeight = "100vh";
        content.style.pointerEvents = "auto";
        content.style.padding = "1em 2em";
        upItem.style.display = "inline-block";
        downItem.style.display = "none";
        title.style.color = "#383737";
    } else {
        content.style.opacity = "0";
        content.style.maxHeight = "0";
        content.style.pointerEvents = "none";
        content.style.padding = "0.5em 2em";
        upItem.style.display = "none";
        downItem.style.display = "inline-block";
        title.style.color = "#a0a0a0";
    }
}
function showPage(onPage) {
    let pages = document.querySelectorAll('.AccountContent > div');
    pages.forEach(page => {
        page.style.display = "none";
    });

    document.getElementById(onPage).style.display = "flex";
}