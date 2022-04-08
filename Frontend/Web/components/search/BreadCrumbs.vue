<template>
    <div class="bread-crumbs">
        <span class="crumb crumb-button" @click="back()">&larr; Back To Results</span> <span class="divider">|</span>
        <!-- <span class="crumb button">{{listing.listingType}}</span> <span class="divider">|</span> -->
        <span class="crumb">{{listing.listingType}}</span> <span class="divider">|</span>
        <span class="crumb">{{listing.regionOrState}}</span> <span class="divider">|</span>
        <span class="crumb">{{listing.propertyType}}</span> <span class="divider">|</span>
        <span class="crumb">{{listing.address}}</span>
    </div>
</template>

<script>
    export default {
        props: {
            listing: {
            type: Object,
            },
        },
        methods: {
            back() {
                this.$router.go(-1)
            }
        },
    }
</script>

<style lang="scss" scoped>
.bread-crumbs {
    padding: 5px;
    background-color: white;;
    display: flex;
    align-items: center;
    font-size: .8rem;

    .crumb {
        margin-right: 5px;
        margin-left: 5px;
        padding: 5px;
        color: grey;
    }

    .crumb-button {
        cursor: pointer;
        color: blue;

        &:hover {
            color: blue;
            text-decoration: underline;
        }
    }

    .results {
        padding-left: 3px;
        padding-right:3px;
        // margin-right: 10px;
    }

    .divider {
        font-size: 10px;
        color: grey;
    }
}
</style>