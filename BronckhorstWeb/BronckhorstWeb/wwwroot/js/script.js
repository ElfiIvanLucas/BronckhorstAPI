//Toggle class active untuk menu

const navbar = document.querySelector(".navbar");
const menu = document.querySelector("#menu");
const ctgMenu = document.querySelector("#ctg-menu");
const ctgModal = document.querySelector("#ctg-modal");
const ctgModalClose = document.querySelector("#ctg-modal-close");
const ctgSidebarLinks = document.querySelectorAll(".ctg-modal-sidebar a[data-category]");
const ctgModalPanes = document.querySelectorAll(".ctg-modal-pane[data-category]");

function setActiveCategory(category) {
    ctgSidebarLinks.forEach((link) => {
        link.classList.toggle("is-active", link.dataset.category === category);
    });

    ctgModalPanes.forEach((pane) => {
        pane.classList.toggle("is-active", pane.dataset.category === category);
    });
}

// ketika menu di klik

menu.onclick = () => {
    navbar.classList.toggle("active");
}

if (ctgMenu && ctgModal) {
    ctgMenu.addEventListener("click", () => {
        ctgModal.classList.add("active");
        ctgModal.setAttribute("aria-hidden", "false");
        ctgMenu.setAttribute("aria-expanded", "true");
        setActiveCategory("implantaten");
    });
}

ctgSidebarLinks.forEach((link) => {
    link.addEventListener("mouseenter", () => {
        setActiveCategory(link.dataset.category);
    });

    link.addEventListener("focus", () => {
        setActiveCategory(link.dataset.category);
    });

    link.addEventListener("click", (event) => {
        event.preventDefault();
        setActiveCategory(link.dataset.category);
    });
});

const menuButton = document.getElementById("menu");
const navMenu = document.querySelector(".navbar-nav");

if (menuButton && navMenu) {
    menuButton.addEventListener("click", (event) => {
        event.preventDefault();
        navMenu.classList.toggle("active");
    });
}

// Toggle class active untuk search form

const searchForm = document.querySelector(".search-form");
const searchBox = document.querySelector("#search-box");

document.querySelector("#search-button").addEventListener("click", (e) => {
    e.preventDefault();
    searchForm.classList.toggle("active");
    searchBox.focus();
});

// Toggle class active untuk shopping cart

const shoppingCart = document.querySelector(".shopping-cart");
const shoppingCartButton = document.querySelector("#shopping-cart-button");

shoppingCartButton.onclick = (event) => {
    shoppingCart.classList.toggle("active");
    event.preventDefault();
}

// klik di luar elemen


document.addEventListener("click", function (e) {
    const clickedCtgMenu = ctgMenu && ctgMenu.contains(e.target);

    if (!menu.contains(e.target) && !clickedCtgMenu && !navMenu.contains(e.target)) {
        navMenu.classList.remove("active");
    }

    if (!searchForm.contains(e.target) && !document.querySelector("#search-button").contains(e.target)) {
        searchForm.classList.remove("active");
    }

    if (!shoppingCart.contains(e.target) && !shoppingCartButton.contains(e.target)) {
        shoppingCart.classList.remove("active");
    }
});

if (ctgModal && ctgModalClose) {
    ctgModalClose.addEventListener("click", () => {
        ctgModal.classList.remove("active");
        ctgModal.setAttribute("aria-hidden", "true");
        if (ctgMenu) {
            ctgMenu.setAttribute("aria-expanded", "false");
        }
    });

    ctgModal.addEventListener("click", (event) => {
        if (event.target === ctgModal) {
            ctgModal.classList.remove("active");
            ctgModal.setAttribute("aria-hidden", "true");
            if (ctgMenu) {
                ctgMenu.setAttribute("aria-expanded", "false");
            }
        }
    });
}

// modal box
const itemDetailModal1 = document.querySelector("#item-detail-modal1");
const itemDetailButton1 = document.querySelectorAll(".item-detail-button1");

if (itemDetailModal1) {
    itemDetailButton1.forEach((button) => {
        button.onclick = (event) => {
            itemDetailModal1.style.display = "flex";
            event.preventDefault();
        };
    });

    // klik tombol close modal
    const itemDetailCloseButton = document.querySelector("#item-detail-modal1 .close-icon");

    if (itemDetailCloseButton) {
        itemDetailCloseButton.onclick = (event) => {
            itemDetailModal1.style.display = "none";

            event.preventDefault();
        };
    }

    // klik di luar modal
    window.addEventListener("click", (event) => {
        if (event.target === itemDetailModal1) {
            itemDetailModal1.style.display = "none";
        }
    });
}

const itemDetailModal2 = document.querySelector("#item-detail-modal2");
const itemDetailButton2 = document.querySelectorAll(".item-detail-button2");

if (itemDetailModal2) {
    itemDetailButton2.forEach((button) => {
        button.onclick = (event) => {
            itemDetailModal2.style.display = "flex";

            event.preventDefault();
        };
    });

    // klik tombol close modal
    const itemDetailCloseButton = document.querySelector("#item-detail-modal2 .close-icon");

    if (itemDetailCloseButton) {
        itemDetailCloseButton.onclick = (event) => {
            itemDetailModal2.style.display = "none";
            
            event.preventDefault();
        };
    }

    // klik di luar modal
    window.addEventListener("click", (event) => {
        if (event.target === itemDetailModal2) {
            itemDetailModal2.style.display = "none";
        }
    });
}