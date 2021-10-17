using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script
{
    public class GameConstants
    {
        public const string TAG_PLAYER = "Player";
        public const string TAG_WATER =  "water";
        public const string TAG_MOVING_OBJECT = "moving_object";
        public const string TAG_DAMAGE_OBJECT = "damage_object";
        public const string TAG_PLAYER_DAMAGE_OBJECT = "player_damage_object";
        public const string TAG_REPEL_OBJECT = "repel_object";
        public const string TAG_INDIRECT_DAMAGE_OBJECT = "indirect_damage_object";
        public const string TAG_INDIRECT_PLAYER_DAMAGE_OBJECT = "indirect_player_damage_object";
        public const string TAG_INDIRECT_REPEL_OBJECT = "indirect_repel_object";

        public const string NAME_PLAYER = "player";
        public const string NAME_PLAYER_CAMERA = "player_camera";
        public const string NAME_GAME_SCENE_DATA = "scene_data";

        public const string TAG_PLAYER_CAMERA_TARGET = "player_camera_target";
        public const string TAG_MAIN_CAMERA = "MainCamera";

        public const string DIRECTORY_FONT = "font/game_font";

        public const int LAYER_MASK_ONLY_PLAYER = 1 << 8;
        public const int LAYER_MASK_ALL_BUT_PLAYER = ~(1 << 8);
        public const int LAYER_MASK_ALL_BUT_ENTITIES = ~((1 << 8) | (1 << 9) | (1 << 10) | (1 << 11));

        public const string EVENT_TYPE_NULL = "null";
        public const string EVENT_TYPE_DELAY = "delay";
        public const string EVENT_TYPE_MESSAGE_BOX = "message_box";
        public const string EVENT_TYPE_MESSAGE_BOX_QUESTION = "message_box_question";
        public const string EVENT_TYPE_SET_CAMERA = "set_camera";
        public const string EVENT_TYPE_SET_PLAYER_ANIMATOR = "set_player_animator";
        public const string EVENT_TYPE_SET_PLAYER_EYE = "set_player_eye";
        public const string EVENT_TYPE_UNSET_PLAYER_EYE = "unset_player_eye";
        public const string EVENT_TYPE_SET_ACTOR_ANIMATOR = "set_actor_animator";
        public const string EVENT_TYPE_UNSET_CAMERA = "unset_camera";
        public const string EVENT_TYPE_SAVE_GAME = "save_game";
        public const string EVENT_TYPE_CONDITIONAL_INT = "conditional_int";
        public const string EVENT_TYPE_PLAY_MUSIC = "play_music";
        

        public enum CameraMode
        {
            camera_default,
            camera_fixed,
            camera_fixed_tracking
        }

        public enum DamageSourceType
        {
            type_static,
            type_actor
        }

        public enum DamageEffectType
        {
            type_default,
            type_fire
        }

        public enum DamageDirectionType
        {
            type_up,
            type_down,
            type_push
        }

        public enum GroundType
        {
            ground_default,
            ground_slide,
            ground_water,
            ground_grass,
            ground_sand,
            ground_stone,
            ground_wood,
            ground_mud,
            ground_metal,
            ground_foliage,
        }

    }

    

}
