using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace Optimus.STFC.ConfigManager.Models
{
    public class CommunityModSettingsTomlModel
    {
        [DataMember(Name = "buffs")]
        public Buffs? buffs { get; set; }

        [DataMember(Name = "control")]
        public Control? control { get; set; }

        [DataMember(Name = "graphics")]
        public Graphics? graphics { get; set; }

        [DataMember(Name = "sync")]
        public Sync? sync { get; set; }

        [DataMember(Name = "tech")]
        public Tech? tech { get; set; }

        [DataMember(Name = "ui")]
        public Ui? ui { get; set; }

        [DataMember(Name = "shortcuts")]
        public Shortcuts? shortcuts { get; set; }

        [DataMember(Name = "config")]
        public Config? config { get; set; }

        public class Buffs
        {
            [DataMember(Name = "use_out_of_dock_power")]
            public bool? use_out_of_dock_power { get; set; }
        }

        public class Control
        {
            [DataMember(Name = "hotkeys_enabled")]
            public bool? hotkeys_enabled { get; set; }

            [DataMember(Name = "hotkeys_extended")]
            public bool? hotkeys_extended { get; set; }

            [DataMember(Name = "use_scopely_hotkeys")]
            public bool? use_scopely_hotkeys { get; set; }
        }

        public class Config
        {
            [DataMember(Name = "assets_url_override")]
            public string? assets_url_override { get; set; }

            [DataMember(Name = "settings_url")]
            public string? settings_url { get; set; }
        }

        public class Graphics
        {
            [DataMember(Name = "adjust_scale_res")]
            public bool? adjust_scale_res { get; set; }

            [DataMember(Name = "borderless_fullscreen_f11")]
            public bool? borderless_fullscreen_f11 { get; set; }

            [DataMember(Name = "default_system_zoom")]
            public float? default_system_zoom { get; set; }

            [DataMember(Name = "free_resize")]
            public bool? free_resize { get; set; }

            [DataMember(Name = "keyboard_zoom_speed")]
            public float? keyboard_zoom_speed { get; set; }

            [DataMember(Name = "show_all_resolutions")]
            public bool? show_all_resolutions { get; set; }

            [DataMember(Name = "system_pan_momentum_falloff")]
            public float? system_pan_momentum_falloff { get; set; }

            [DataMember(Name = "system_zoom_preset_1")]
            public float? system_zoom_preset_1 { get; set; }

            [DataMember(Name = "system_zoom_preset_2")]
            public float? system_zoom_preset_2 { get; set; }

            [DataMember(Name = "system_zoom_preset_3")]
            public float? system_zoom_preset_3 { get; set; }

            [DataMember(Name = "system_zoom_preset_4")]
            public float? system_zoom_preset_4 { get; set; }

            [DataMember(Name = "system_zoom_preset_5")]
            public float? system_zoom_preset_5 { get; set; }

            [DataMember(Name = "target_framerate")]
            public int? target_framerate { get; set; }

            [DataMember(Name = "transition_time")]
            public float? transition_time { get; set; }

            [DataMember(Name = "ui_scale")]
            public float? ui_scale { get; set; }

            [DataMember(Name = "ui_scale_adjust")]
            public float? ui_scale_adjust { get; set; }

            [DataMember(Name = "ui_scale_viewer")]
            public float? ui_scale_viewer { get; set; }

            [DataMember(Name = "use_presets_as_default")]
            public bool? use_presets_as_default { get; set; }

            [DataMember(Name = "vsync")]
            public int? vsync { get; set; }

            [DataMember(Name = "zoom")]
            public float? zoom { get; set; }
        }

        public class Sync
        {
            
            //[DataMember(Name = "token")]
            //public string? token { get; set; }

            //[DataMember(Name = "url")]
            //public string? url { get; set; }

            [DataMember(Name = "file")]
            public string? file { get; set; }

            [DataMember(Name = "proxy")]
            public string? proxy { get; set; }

            [DataMember(Name = "logging")]
            public bool? logging { get; set; }

            [DataMember(Name = "battlelogs")]
            public bool? battlelogs { get; set; }



            [DataMember(Name = "buildings")]
            public bool? buildings { get; set; }

            [DataMember(Name = "missions")]
            public bool? missions { get; set; }

            [DataMember(Name = "officer")]
            public bool? officer { get; set; }

            [DataMember(Name = "research")]
            public bool? research { get; set; }

            [DataMember(Name = "resources")]
            public bool? resources { get; set; }

            [DataMember(Name = "ships")]
            public bool? ships { get; set; }

            [DataMember(Name = "tech")]
            public bool? tech { get; set; }

            [DataMember(Name = "traits")]
            public bool? traits { get; set; }

            [DataMember(Name = "targets")]
            public SyncTargets? targets { get; set; }

            public class SyncTargets
            { 
                /// to do
            }
        }

        public class Tech
        {
            [DataMember(Name = "fix_unity_web_requests")]
            public bool? fix_unity_web_requests { get; set; }
        }

        public class Ui
        {
            [DataMember(Name = "always_skip_reveal_sequence")]
            public bool? always_skip_reveal_sequence { get; set; }

            [DataMember(Name = "disable_escape_exit")]
            public bool? disable_escape_exit { get; set; }

            [DataMember(Name = "disable_first_popup")]
            public bool? disable_first_popup { get; set; }

            [DataMember(Name = "disable_galaxy_chat")]
            public bool? disable_galaxy_chat { get; set; }

            [DataMember(Name = "disable_toast_banners")]
            public bool? disable_toast_banners { get; set; }

            [DataMember(Name = "disabled_banner_types")]
            public string? disabled_banner_types { get; set; }

            [DataMember(Name = "extend_donation_slider")]
            public bool? extend_donation_slider { get; set; }

            [DataMember(Name = "show_cargo_default")]
            public bool? show_cargo_default { get; set; }

            [DataMember(Name = "show_player_cargo")]
            public bool? show_player_cargo { get; set; }

            [DataMember(Name = "show_station_cargo")]
            public bool? show_station_cargo { get; set; }

            [DataMember(Name = "show_hostile_cargo")]
            public bool? show_hostile_cargo { get; set; }

            [DataMember(Name = "show_armada_cargo")]
            public bool? show_armada_cargo { get; set; }

            [DataMember(Name = "stay_in_bundle_after_summary")]
            public bool? stay_in_bundle_after_summary { get; set; }

            [DataMember(Name = "disable_move_keys")]
            public bool? disable_move_keys { get; set; }

            [DataMember(Name = "disable_preview_locate")]
            public bool? disable_preview_locate { get; set; }

            [DataMember(Name = "disable_preview_recall")]
            public bool? disable_preview_recall { get; set; }

            [DataMember(Name = "extend_donation_max")]
            public int? extend_donation_max { get; set; }
        }

        public class Shortcuts
        {
            [DataMember(Name = "action_primary")]
            public string? action_primary { get; set; }

            [DataMember(Name = "action_recall")]
            public string? action_recall { get; set; }

            [DataMember(Name = "action_recall_cancel")]
            public string? action_recall_cancel { get; set; }

            [DataMember(Name = "action_repair")]
            public string? action_repair { get; set; }

            [DataMember(Name = "action_secondary")]
            public string? action_secondary { get; set; }

            [DataMember(Name = "action_view")]
            public string? action_view { get; set; }

            [DataMember(Name = "hotkeys_disble")]
            public string? hotkeys_disble { get; set; }

            [DataMember(Name = "hotkeys_enable")]
            public string? hotkeys_enable { get; set; }

            [DataMember(Name = "log_debug")]
            public string? log_debug { get; set; }

            [DataMember(Name = "log_info")]
            public string? log_info { get; set; }

            [DataMember(Name = "log_trace")]
            public string? log_trace { get; set; }

            [DataMember(Name = "select_chatalliance")]
            public string? select_chatalliance { get; set; }

            [DataMember(Name = "select_chatglobal")]
            public string? select_chatglobal { get; set; }

            [DataMember(Name = "select_chatprivate")]
            public string? select_chatprivate { get; set; }

            [DataMember(Name = "select_ship1")]
            public string? select_ship1 { get; set; }

            [DataMember(Name = "select_ship2")]
            public string? select_ship2 { get; set; }

            [DataMember(Name = "select_ship3")]
            public string? select_ship3 { get; set; }

            [DataMember(Name = "select_ship4")]
            public string? select_ship4 { get; set; }

            [DataMember(Name = "select_ship5")]
            public string? select_ship5 { get; set; }

            [DataMember(Name = "select_ship6")]
            public string? select_ship6 { get; set; }

            [DataMember(Name = "select_ship7")]
            public string? select_ship7 { get; set; }

            [DataMember(Name = "select_ship8")]
            public string? select_ship8 { get; set; }

            [DataMember(Name = "show_alliance")]
            public string? show_alliance { get; set; }

            [DataMember(Name = "show_artifacts")]
            public string? show_artifacts { get; set; }

            [DataMember(Name = "show_awayteam")]
            public string? show_awayteam { get; set; }

            [DataMember(Name = "show_bookmarks")]
            public string? show_bookmarks { get; set; }

            [DataMember(Name = "show_chat")]
            public string? show_chat { get; set; }

            [DataMember(Name = "show_chatside1")]
            public string? show_chatside1 { get; set; }

            [DataMember(Name = "show_chatside2")]
            public string? show_chatside2 { get; set; }

            [DataMember(Name = "show_commander")]
            public string? show_commander { get; set; }

            [DataMember(Name = "show_daily")]
            public string? show_daily { get; set; }

            [DataMember(Name = "show_events")]
            public string? show_events { get; set; }

            [DataMember(Name = "show_exocomp")]
            public string? show_exocomp { get; set; }

            [DataMember(Name = "show_factions")]
            public string? show_factions { get; set; }

            [DataMember(Name = "show_galaxy")]
            public string? show_galaxy { get; set; }

            [DataMember(Name = "show_gifts")]
            public string? show_gifts { get; set; }

            [DataMember(Name = "show_inventory")]
            public string? show_inventory { get; set; }

            [DataMember(Name = "show_missions")]
            public string? show_missions { get; set; }

            [DataMember(Name = "show_officers")]
            public string? show_officers { get; set; }

            [DataMember(Name = "show_qtrials")]
            public string? show_qtrials { get; set; }

            [DataMember(Name = "show_refinery")]
            public string? show_refinery { get; set; }

            [DataMember(Name = "show_research")]
            public string? show_research { get; set; }

            [DataMember(Name = "show_ships")]
            public string? show_ships { get; set; }

            [DataMember(Name = "show_stationexterior")]
            public string? show_stationexterior { get; set; }

            [DataMember(Name = "show_stationinterior")]
            public string? show_stationinterior { get; set; }

            [DataMember(Name = "show_system")]
            public string? show_system { get; set; }

            [DataMember(Name = "toggle_cargo_armada")]
            public string? toggle_cargo_armada { get; set; }

            [DataMember(Name = "toggle_cargo_default")]
            public string? toggle_cargo_default { get; set; }

            [DataMember(Name = "toggle_cargo_hostile")]
            public string? toggle_cargo_hostile { get; set; }

            [DataMember(Name = "toggle_cargo_player")]
            public string? toggle_cargo_player { get; set; }

            [DataMember(Name = "toggle_cargo_station")]
            public string? toggle_cargo_station { get; set; }

            [DataMember(Name = "toggle_preview_locate")]
            public string? toggle_preview_locate { get; set; }

            [DataMember(Name = "ui_scaledown")]
            public string? ui_scaledown { get; set; }

            [DataMember(Name = "ui_scaleup")]
            public string? ui_scaleup { get; set; }

            [DataMember(Name = "zoom_preset1")]
            public string? zoom_preset1 { get; set; }

            [DataMember(Name = "zoom_preset2")]
            public string? zoom_preset2 { get; set; }

            [DataMember(Name = "zoom_preset3")]
            public string? zoom_preset3 { get; set; }

            [DataMember(Name = "zoom_preset4")]
            public string? zoom_preset4 { get; set; }

            [DataMember(Name = "zoom_preset5")]
            public string? zoom_preset5 { get; set; }

            [DataMember(Name = "set_zoom_default")]
            public string? set_zoom_default { get; set; }

            [DataMember(Name = "set_zoom_preset1")]
            public string? SetZoomPreset1 { get; set; }

            [DataMember(Name = "set_zoom_preset2")]
            public string? SetZoomPreset2 { get; set; }

            [DataMember(Name = "set_zoom_preset3")]
            public string? SetZoomPreset3 { get; set; }

            [DataMember(Name = "set_zoom_preset4")]
            public string? SetZoomPreset4 { get; set; }

            [DataMember(Name = "set_zoom_preset5")]
            public string? SetZoomPreset5 { get; set; }

            [DataMember(Name = "zoom_in")]
            public string? zoom_in { get; set; }

            [DataMember(Name = "zoom_out")]
            public string? zoom_out { get; set; }

            [DataMember(Name = "zoom_max")]
            public string? zoom_max { get; set; }

            [DataMember(Name = "zoom_min")]
            public string? zoom_min { get; set; }

            [DataMember(Name = "zoom_reset")]
            public string? zoom_reset { get; set; }

            [DataMember(Name = "ui_scaleviewerdown")]
            public string? ui_scaleviewerdown { get; set; }

            [DataMember(Name = "ui_scaleviewerup")]
            public string? ui_scaleviewerup { get; set; }

        }

    }


}
