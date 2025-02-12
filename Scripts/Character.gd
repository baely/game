extends AnimatedSprite

var grid_size = 16
var is_moving = false
var target_position = Vector2.ZERO
var speed = 50
var last_direction = "down"  # Track last direction for idle state

func _ready():
	position = position.snapped(Vector2.ONE * grid_size)
	target_position = position
	play("idle")  # Default idle front

func update_animation(direction: String, is_walking: bool):
	var animation_prefix = "walk" if is_walking else "idle"
	
	match direction:
		"left":
			play(animation_prefix + "_side")
			flip_h = true
		"right":
			play(animation_prefix + "_side")
			flip_h = false
		"up":
			play(animation_prefix + "_back")
		"down":
			play(animation_prefix)  # Using default animations for front
	
	last_direction = direction

func _process(delta):
	if is_moving:
		var move_direction = (target_position - position).normalized()
		position += move_direction * speed * delta
		
		if position.distance_to(target_position) < 1:
			position = target_position
			is_moving = false
			# When stopping, play idle animation for last direction
			update_animation(last_direction, false)
	else:
		var direction = Vector2.ZERO
		var dir_string = last_direction
		
		if Input.is_action_pressed("ui_right"):
			direction.x += 1
			dir_string = "right"
		elif Input.is_action_pressed("ui_left"):
			direction.x -= 1
			dir_string = "left"
		elif Input.is_action_pressed("ui_up"):
			direction.y -= 1
			dir_string = "up"
		elif Input.is_action_pressed("ui_down"):
			direction.y += 1
			dir_string = "down"
			
		if direction != Vector2.ZERO:
			target_position = position + (direction * grid_size)
			is_moving = true
			update_animation(dir_string, true)
