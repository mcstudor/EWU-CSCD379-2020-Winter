import '../styles/site.scss';
import Vue from 'vue';
import UserComponent from './components/User/userComponent.vue';
import GiftComponent from './components/Gift/giftComponent.vue';
import GroupComponent from './components/Group/groupComponent.vue';


document.addEventListener("DOMContentLoaded", async () => {
    if(document.getElementById('userComponent')){
        new Vue({
            render: h => h(UserComponent)
        }).$mount('#userComponent');
    } else if(document.getElementById('giftComponent')){
        new Vue({
            render: h => h(GiftComponent)
        }).$mount('#giftComponent');
    } else if(document.getElementById('groupComponent')){
        new Vue({
            render: h => h(GroupComponent)
        }).$mount('#groupComponent');
    };
});