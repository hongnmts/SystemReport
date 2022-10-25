<script>
import { layoutMethods } from "@/state/helpers";

import { menuItems } from "./menu-items";
/**
 * Horizontal navbar component
 */
export default {
  data() {
    return {
      menuItems: menuItems,
    };
  },
  mounted() {
    var links = document.getElementsByClassName("side-nav-link");
    var matchingMenuItem = null;
    for (var i = 0; i < links.length; i++) {
      if (window.location.pathname === links[i].pathname) {
        matchingMenuItem = links[i];
        break;
      }
    }

    if (matchingMenuItem) {
      matchingMenuItem.classList.add("active");
      var parent = matchingMenuItem.parentElement;

      /**
       * TODO: This is hard coded way of expading/activating parent menu dropdown and working till level 3.
       * We should come up with non hard coded approach
       */
      if (parent) {
        parent.classList.add("active");
        const parent2 = parent.parentElement;
        if (parent2) {
          parent2.classList.add("active");
        }
        const parent3 = parent2.parentElement;
        if (parent3) {
          parent3.classList.add("active");
          var childAnchor = parent3.querySelector(".has-dropdown");
          if (childAnchor) childAnchor.classList.add("active");
        }

        const parent4 = parent3.parentElement;
        if (parent4) parent4.classList.add("active");
        const parent5 = parent4.parentElement;
        if (parent5) parent5.classList.add("active");
      }
    }
  },
  methods: {
    ...layoutMethods,

    /**
     * menu clicked show the subelement
     */
    onMenuClick(event) {
      event.preventDefault();
      const nextEl = event.target.nextSibling;
      if (nextEl && !nextEl.classList.contains("show")) {
        const parentEl = event.target.parentNode;
        if (parentEl) {
          parentEl.classList.remove("show");
        }
        nextEl.classList.add("show");
      } else if (nextEl) {
        nextEl.classList.remove("show");
      }
      return false;
    },
    topbarLight() {
      document.body.setAttribute("data-topbar", "light");
      document.body.removeAttribute("data-layout-size", "boxed");
    },
    boxedWidth() {
      document.body.setAttribute("data-layout-size", "boxed");
      document.body.removeAttribute("data-topbar", "light");
      document.body.setAttribute("data-topbar", "dark");
    },
    changeLayout(layout) {
      this.changeLayoutType({ layoutType: layout });
    },
    /**
     * Returns true or false if given menu item has child or not
     * @param item menuItem
     */
    hasItems(item) {
      return item.subItems !== undefined ? item.subItems.length > 0 : false;
    },
    handleClickIsActive(event, id){
      this.menuItems.map((value, index) => {
        if(value.id == id){
          value.isActve = true;
        }
      })
    },
  },
};
</script>
<template>
  <div class="bg-navbar">
    <section id="navbar-secondary" class="container-fluid p-0 my-3">
      <nav class="navbar navbar-light navbar-expand-lg topnav-menu p-0">
        <div class="collapse navbar-collapse" id="topnav-menu-content">
          <ul class="navbar-nav">
            <li
                v-for="(item, index) in menuItems"
                :key="index"
                class="nav-item dropdown ms-2"
            >
              <router-link
                  :to="item.link"
                  class="nav-link dropdown-toggle arrow-none side-nav-link"
              >
                <i :class="`mdi ${item.icon} icon-item`"></i>
                <span class="px-2">{{item.label}}</span>
              </router-link>
            </li>
          </ul>
        </div>
      </nav>
    </section>
  </div>
</template>

<style>

#navbar-secondary li .nav-link {
  color: #111961;
  background-color: #fff;
  border-radius: 50px;
  padding: 12px 10px;
}
#navbar-secondary li .nav-link .icon-item {
  color: #fff;
  background-color: #111961;
  padding: 5px 7px;
  border-radius: 50px;
}

#navbar-secondary li .nav-link.router-link-exact-active{
  background-color: #05afc8;
  color: #fff;
}

#navbar-secondary li .nav-link.router-link-exact-active  .icon-item{
  background-color: #fff;
  color: #111961;
}

</style>
