using System;

namespace ResourceTypes.Collisions
{
    //taken from PhysX sdk
    [Flags]
    public enum MeshSerialFlags
    {
        MSF_MATERIALS = (1 << 0),
        MSF_FACE_REMAP = (1 << 1),
        MSF_HARDWARE_MESH = (1 << 2),
        MSF_8BIT_INDICES = (1 << 3),
        MSF_16BIT_INDICES = (1 << 4),
    }

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
