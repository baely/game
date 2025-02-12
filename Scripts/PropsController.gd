extends Node2D

var rock_positions = []
var vegetation_positions = []
var water_plant_positions = []
var time = 0
var wind_influence = Vector2.ZERO

func _ready():
	# Get TileMap nodes to find valid positions
	var town_map = get_node("/root/Main/Town")
	var water_positions = []
	var land_positions = []
	
	# Connect to wind controller
	var wind_controller = get_node("/root/Main/WindController")
	if wind_controller:
		wind_controller.connect("wind_changed", self, "_on_wind_changed")
	
	# Scan tilemap for valid positions
	if town_map:
		var used_cells = town_map.get_used_cells()
		for cell in used_cells:
			var cell_id = town_map.get_cell(cell.x, cell.y)
			var world_pos = town_map.map_to_world(cell)
			
			if cell_id == 26:  # Water tile
				water_positions.append(world_pos)
			elif cell_id == 25:  # Land tile
				land_positions.append(world_pos)
	
	# Place props
	_place_props(land_positions, water_positions)

func _place_props(land_positions, water_positions):
	# Place rocks on land
	for i in range(5):
		if land_positions.size() > 0:
			var pos_index = randi() % land_positions.size()
			var pos = land_positions[pos_index]
			_add_rock(pos)
			land_positions.remove(pos_index)
	
	# Place vegetation on land
	for i in range(8):
		if land_positions.size() > 0:
			var pos_index = randi() % land_positions.size()
			var pos = land_positions[pos_index]
			_add_vegetation(pos)
			land_positions.remove(pos_index)
	
	# Place water plants in water
	for i in range(4):
		if water_positions.size() > 0:
			var pos_index = randi() % water_positions.size()
			var pos = water_positions[pos_index]
			_add_water_plant(pos)
			water_positions.remove(pos_index)

func _process(delta):
	time += delta
	
	# Update vegetation movement with wind influence
	for plant in $Vegetation.get_children():
		var original_points = PoolVector2Array([Vector2(0, -12), Vector2(8, 4), Vector2(-8, 4)])
		var new_points = PoolVector2Array()
		
		# Calculate wind-based sway
		var wind_sway = wind_influence.length() * 2
		var wind_angle = wind_influence.angle()
		
		for point in original_points:
			var distance_factor = abs(point.y + 12) / 24.0  # More sway at the top
			var sway = sin(time * (1 + wind_influence.length()) + plant.position.x * 0.1)
			var offset = Vector2(
				cos(wind_angle) * sway * wind_sway * distance_factor,
				sin(wind_angle) * sway * wind_sway * distance_factor
			)
			new_points.append(point + offset)
		plant.polygon = new_points
	
	# Update water plants with gentle sway and wind influence
	for plant in $WaterPlants.get_children():
		var original_points = PoolVector2Array([Vector2(-4, -8), Vector2(4, -8), Vector2(0, 8)])
		var new_points = PoolVector2Array()
		
		var water_sway = 0.3 + wind_influence.length() * 0.5
		for point in original_points:
			var sway = sin(time * 0.5 + plant.position.x * 0.2)
			var offset = Vector2(
				point.x + sway * water_sway * 4,
				point.y + cos(time + plant.position.x * 0.1) * water_sway * 2
			)
			new_points.append(offset)
		plant.polygon = new_points
		
		# Adjust water plant opacity based on movement
		plant.color.a = 0.7 - wind_influence.length() * 0.2

func _add_rock(pos):
	var polygon = Polygon2D.new()
	polygon.position = pos
	polygon.polygon = PoolVector2Array([Vector2(-8, -8), Vector2(8, -8), Vector2(8, 8), Vector2(-8, 8)])
	polygon.color = Color(0.5, 0.5, 0.5, 1)
	$Rocks.add_child(polygon)
	rock_positions.append(pos)

func _add_vegetation(pos):
	var polygon = Polygon2D.new()
	polygon.position = pos
	polygon.polygon = PoolVector2Array([Vector2(0, -12), Vector2(8, 4), Vector2(-8, 4)])
	polygon.color = Color(0.2, 0.8, 0.2, 1)
	$Vegetation.add_child(polygon)
	vegetation_positions.append(pos)

func _add_water_plant(pos):
	var polygon = Polygon2D.new()
	polygon.position = pos
	polygon.polygon = PoolVector2Array([Vector2(-4, -8), Vector2(4, -8), Vector2(0, 8)])
	polygon.color = Color(0.2, 0.6, 0.3, 0.7)
	$WaterPlants.add_child(polygon)
	water_plant_positions.append(pos)

func _on_wind_changed(strength, direction):
	wind_influence = direction * strength
