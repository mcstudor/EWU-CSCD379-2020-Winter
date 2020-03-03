<template>
    <div>
        <button class="button" @click="createUser()">Create New</button>
        <table class="table">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                    <tr v-for="user in users" v-bind:id="user.id">
                        <td>{{user.id}}</td>
                        <td>{{user.firstName}}</td>
                        <td>{{user.lastName}}</td>
                        <td>
                            <button class='button' @click='setUser(user)'>Edit</button> |
                            <button class='button delete-item' @click='deleteUser(user)'>Delete</button>
                        </td>
                    </tr>
            </tbody>
        </table>
        <user-detail-component v-if="selectedUser != null" v-bind:user="selectedUser" @user-saved='refreshUsers()'></user-detail-component>
    </div>
</template>
<script lang="ts">
    import { Vue, Component } from 'vue-property-decorator';
    import { User, UserClient } from '../../secretsanta-client';
    import  UserDetailComponent  from './userDetailComponent.vue';
    @Component({
        components: {
               UserDetailComponent
            }

    })
    export default class UserComponent extends Vue { 
        users : User[] = null;
        selectedUser : User = null;

        async loadUsers() {
            let userClient = new UserClient();
            this.users = await userClient.getAll();
        }

        createUser() {
            this.selectedUser = <User>{};

        }

        async mounted(){
            await this.loadUsers();
        }

        async refreshUsers() {
            this.selectedUser = null;
            await this.loadUsers();
        }

        async deleteUser(user : User){
            let userClient = new UserClient();
            if(confirm(`Are you sure you want to delete ${user.firstName} ${user.lastName}`)){
                await userClient.delete(user.id);                
            };

            await this.refreshUsers();


        }

        setUser(user : User){
            this.selectedUser = user;
        }

        constructor(){
            super();
        }
    }
</script>