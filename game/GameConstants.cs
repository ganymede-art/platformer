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

        public const int LAYER_PLAYER = 8;
        public const int LAYER_ACTOR = 9;
        public const int LAYER_ENEMY = 10;
        public const int LAYER_IGNORE_CAMERA = 11;
        public const int LAYER_ENEMY_BOUNDARY = 12;

        public const int MASK_ONLY_PLAYER = 1 << LAYER_PLAYER;
        public const int MASK_ALL_BUT_PLAYER = ~(1 << LAYER_PLAYER);
        public const int MASK_PLAYER_IGNORES = 
            ~(
                (1 << LAYER_PLAYER)        |
                (1 << LAYER_ACTOR)         |
                (1 << LAYER_ENEMY)         |
                (1 << LAYER_IGNORE_CAMERA) |
                (1 << LAYER_ENEMY_BOUNDARY)
            );
        public const int MASK_ENEMY_IGNORES =
            ~(
                (1 << LAYER_PLAYER) |
                (1 << LAYER_ACTOR) |
                (1 << LAYER_ENEMY) |
                (1 << LAYER_IGNORE_CAMERA)
            );

        // event type.

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
        public const string EVENT_TYPE_PLAY_SOUND = "play_sound";
        public const string EVENT_TYPE_SET_PLAYER_ABILITY = "set_player_ability";
        public const string EVENT_TYPE_SET_OBJECT_ACTIVE = "set_object_active";
        public const string EVENT_TYPE_SET_PLAYER_FACE_DIRECTION = "set_player_face_direction";
        public const string EVENT_TYPE_UNSET_PLAYER_FACE_DIRECTION = "unset_player_face_direction";
        public const string EVENT_TYPE_SET_GAME_VAR_BOOL = "set_game_var_bool";
        public const string EVENT_TYPE_MOVE_OBJECT = "move_object";
        public const string EVENT_TYPE_CONDITIONAL_BOOL = "conditional_bool";
        public const string EVENT_TYPE_SET_GAME_VAR_INT = "set_game_var_int";

        // ground type.

        public const string GROUND_TYPE_DEFAULT = "default";
        public const string GROUND_TYPE_GRASS = "grass";
        public const string GROUND_TYPE_SAND = "sand";
        public const string GROUND_TYPE_MUD = "mud";
        public const string GROUND_TYPE_FOLIAGE = "foliage";
        public const string GROUND_TYPE_METAL = "metal";
        public const string GROUND_TYPE_STONE = "stone";
        public const string GROUND_TYPE_WOOD = "wood";
        public const string GROUND_TYPE_WATER = "water";

        public enum CameraMode
        {
            camera_default,
            camera_fixed,
            camera_fixed_tracking
        }
    }

    

}
