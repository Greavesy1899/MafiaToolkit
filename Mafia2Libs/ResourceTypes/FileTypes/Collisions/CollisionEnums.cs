using System;

namespace ResourceTypes.Collisions
{

    // TODO: most likely material indices/names should be taken
    // from the "pc/sds/tables/tables.sds/tables/MaterialsPhysics.tbl" table,
    // where materialIndex = guid & 0xFFFFFFFF; (see list below)
    // This list should be tested in game.

    // One more research note: 
    // if indices from the list and actual result in the game are not match (at least for static collisions),
    // try to add or subtract 2, because in ResourceTypes.Collisions.Collision.Section type
    // the material index is subtracted by 2 for some unknown reason... it can do some trick)

    /*
    // Ultimately the names must be translated into English I guess)
    enum MaterialsFromMaterialsPhysicsTbl
    {
        undefined = -1,
        silnice = 1,
        silnice_prasna = 2,
        prechod_pro_chodce = 3,
        kocici_hlavy = 4,
        chodnik = 5,
        dlazdice = 6,
        trava = 7,
        hlina = 8,
        sterk = 9,
        pisek = 10,
        blato = 11,
        kaluz = 12,
        voda = 13,
        snih = 14,
        kov = 15,
        plech = 16,
        pletivo = 17,
        zabradli = 18,
        drevo = 19,
        koberec = 20,
        drevo_prkna = 21,
        parkety = 22,
        skripavy_beton = 23,
        kachlicky = 24,
        zed = 25,
        omitka = 26,
        cihly = 27,
        sklo_rozbitelne_1 = 28,
        sklo_rozbitelne_2 = 29,
        sklo_neprustrelne = 30,
        kere_stromy = 31,
        universal_tvrdy = 32,
        universal_meky = 33,
        panak = 34,
        no_shot_coll = 35,
        papir = 36,
        calouneni = 37,
        platena_latka = 38,
        camera_coll = 39,
        player_coll = 40,
        sicily_zed = 41,
        trava_trashy = 42,
        trava_negen = 43,
        trava_trashy_negen = 44,
        chodnik_human = 45,
        auto = 46,
        panak_headshot = 47,
        panak_noha = 48,
        panak_ruka = 49,
        trava_sicily = 50,
        hedgerow = 51,
        dno = 52,
        kanal = 53,
        silnice_ky = 54,
        silnice_prasna_ky = 55,
        kocici_hlavy_ky = 56,
        chodnik_ky = 57,
        dlazdice_ky = 58,
        drevo_prkna_ky = 59,
        silnice_tunel = 60,
        zabradli_beton = 61,
        zabradli_drevo = 62,
        papunddeckel = 63
    }
    */

    //sand = grass/snow;
    //plaster is some kind of concrete.
    //car = concrete again?
    //heads_of_cats = concrete; tarmac?; roads use this material.
    //ground_loam = usually used as stone paths; found in hillwood close to the observatory
    //railing = barriers; metal; etc
    //road_ky = Like a dirt; but it reduces you to walking speeds...cars drive slow too.
    //glassbreak_bulletproof = stone walls.
    //glass_break1 = top of stone walls.
    //tiles_ky = tarmac; found on driveways
    //person_leg = edge of sidewalk.
    //sheet_metal = water, puddle sounds.
    //carpet = rock.
    //cloth = invisible barrier;
    //water = gravel track
    public enum CollisionMaterials
    {
        OBSOLETE_Road,
        OBSOLETE_Road_Dusty,
        OBSOLETE_Pedestrian_Crossing,
        Tarmac,
        OBSOLETE_Sidewalk,
        OBSOLETE_Tiles1,
        OBSOLETE_Grass,
        Sidewalk,
        Stairs,
        GrassAndSnow,
        Mud,
        Ballast,
        Gravel,
        OBSOLETE_Snow,
        OBSOLETE_Metal,
        Water,
        OBSOLETE_Mesh,
        MetalRailing,
        Signboard,
        OBSOLETE_Carpet,
        Metal,
        Wood,
        OBSOLETE_Gritty_Concrete,
        WoodPlanks,
        OBSOLETE_Wall,
        Concrete2,
        OBSOLETE_Brick,
        Concrete,
        OBSOLETE_Glass_Break2,
        OBSOLETE_Glass_BulletProof,
        Glass,
        OBSOLETE_Universal_Hard,
        StorePosters,
        OBSOLETE_Person,
        Rock,
        OBSOLETE_Paper,
        OBSOLETE_Upholstery,
        OBSOLETE_Cloth,
        OBSOLETE_Camera_Coll,
        OBSOLETE_Player_Coll,
        OBSOLETE_Sicily_Wall,
        OBSOLETE_Grass_Trashy,
        PlayerCollision,
        OBSOLETE_Grass_Trashy_Negen,
        OBSOLETE_SideWalk_Human,
        StoneGrassTransition,
        OBSOLETE_Person_Headshot,
        SidewalkEdge,
        OBSOLETE_Person_Hand,
        OBSOLETE_Grass_Sicily,
        OBSOLETE_Hedgerow,
        OBSOLETE_Seabed,
        OBSOLETE_Channel,
        Bush_Foliage,
        OBSOLETE_Road_Dusty_KY,
        OBSOLETE_SideWalk_KY,
        OBSOLETE_Tiles_KY,
        OBSOLETE_Wooden_Board_KY,
        OBSOLETE_Road_Tunnel,
        OBSOLETE_Railing_Concrete,
        OBSOLETE_Railing_Wood,
        OBSOLETE_Cartoon
    }
}
