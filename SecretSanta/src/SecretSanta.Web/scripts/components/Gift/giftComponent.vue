<template>
    <div>
        <button class="button" @click="createGift()">Create New</button>
        <table class="table">
            <thead>
                <tr>
                    <th>Id</th>
                    <th>Title</th>
                    <th>Description</th>
                    <th>Url</th>
                    <th>User Id</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                    <tr v-for="gift in gifts" v-bind:id="gift.id">
                        <td>{{gift.id}}</td>
                        <td>{{gift.title}}</td>
                        <td>{{gift.description}}</td>
                        <td><a :href='gift.url' target='_blank'>{{gift.url}}</a></td>
                        <td>{{gift.userId}}</td>
                        <td>
                            <button class='button' @click='setGift(gift)'>Edit</button> |
                            <button class='button delete-item' @click='deleteGift(gift)'>Delete</button>
                        </td>
                    </tr>
            </tbody>
        </table>
        <gift-detail-component v-if="selectedGift != null" v-bind:gift="selectedGift" @gift-saved='refreshGifts()'></gift-detail-component>
    </div>
</template>
<script lang="ts">
    import { Vue, Component } from 'vue-property-decorator';
    import { Gift, GiftClient } from '../../secretsanta-client';
    import  GiftDetailComponent  from './giftDetailComponent.vue';
    @Component({
        components: {
               GiftDetailComponent
            }

    })
    export default class GiftComponent extends Vue { 
        gifts : Gift[] = null;
        selectedGift : Gift = null;

        async loadGifts() {
            let giftClient = new GiftClient();
            this.gifts = await giftClient.getAll();
        }

        createGift() {
            this.selectedGift = <Gift>{};

        }

        async mounted(){
            await this.loadGifts();
        }

        async refreshGifts() {
            this.selectedGift = null;
            await this.loadGifts();
        }

        async deleteGift(gift : Gift){
            let giftClient = new GiftClient();
            if(confirm(`Are you sure you want to delete ${gift.title}`)){
                await giftClient.delete(gift.id);                
            };

            await this.refreshGifts();
        }

        setGift(gift : Gift){
            this.selectedGift = gift;
        }

        constructor(){
            super();
        }
    }
</script>