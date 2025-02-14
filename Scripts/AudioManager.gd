extends Node

var nature_sounds = {
	"day": ["birds", "wind_leaves"],
	"night": ["crickets", "owls"],
	"rain": ["rain_light", "rain_heavy", "thunder"],
	"water": ["water_lap", "water_splash"]
}

var current_time = "day"
var current_weather = "clear"
var wind_volume = -30.0

func _ready():
	# Start ambient sounds
	play_ambient_sound()
	
	# Set up weather sound timer
	var weather_timer = Timer.new()
	weather_timer.wait_time = 3.0
	weather_timer.connect("timeout", self, "_on_weather_change")
	add_child(weather_timer)
	weather_timer.start()
	
	# Connect to wind controller
	var wind_controller = get_node("/root/Main/WindController")
	if wind_controller:
		wind_controller.connect("wind_changed", self, "_on_wind_changed")
	
	# Add wind audio stream
	var wind_player = AudioStreamPlayer.new()
	wind_player.name = "WindSound"
	wind_player.volume_db = wind_volume
	wind_player.bus = "Ambient"
	add_child(wind_player)

func play_ambient_sound():
	if current_time == "day":
		$AmbientNature.volume_db = -20
		$AmbientNature.pitch_scale = rand_range(0.95, 1.05)
	else:
		$AmbientNature.volume_db = -15
		$AmbientNature.pitch_scale = rand_range(0.9, 1.1)
	
	if !$AmbientNature.playing:
		$AmbientNature.play()

func play_water_sound():
	if !$WaterSounds.playing:
		$WaterSounds.volume_db = -15
		$WaterSounds.pitch_scale = rand_range(0.9, 1.1)
		$WaterSounds.play()

func play_weather_sound():
	match current_weather:
		"Rainy":
			$WeatherSounds.volume_db = -10
			$WeatherSounds.pitch_scale = rand_range(0.95, 1.05)
			$WeatherSounds.play()
		"Stormy":
			$WeatherSounds.volume_db = -5
			$WeatherSounds.pitch_scale = rand_range(0.9, 1.1)
			$WeatherSounds.play()
		_:
			$WeatherSounds.stop()

func _on_weather_change():
	var nearby_water = false  # This should be set based on player position
	if nearby_water:
		play_water_sound()
		
	# Update weather sounds based on HUD weather
	var hud = get_node("/root/Main/HUD")
	if hud:
		current_weather = hud.current_weather
		play_weather_sound()
		
	# Update day/night sounds based on DayNightCycle
	var day_night = get_node("/root/Main/DayNightCycle")
	if day_night:
		var cycle = day_night.time
		current_time = "night" if cycle > 0.75 || cycle < 0.25 else "day"
		play_ambient_sound()

func _on_wind_changed(strength, direction):
	var wind_player = $WindSound
	if wind_player:
		# Adjust wind sound volume based on strength
		var target_volume = lerp(-30.0, -10.0, strength)
		wind_volume = lerp(wind_volume, target_volume, 0.1)
		wind_player.volume_db = wind_volume
		
		# Adjust wind sound pitch based on direction changes
		wind_player.pitch_scale = 1.0 + (strength * 0.2) + (sin(direction.angle()) * 0.1)
		
		if !wind_player.playing:
			wind_player.play()
