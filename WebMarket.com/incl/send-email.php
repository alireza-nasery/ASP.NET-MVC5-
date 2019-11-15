
<?php

/* set the email of the recipient (your email) */
$recipient = "info@proteusthemes.com";


if ( isset($_POST['submit']) ) // just send the email if the $_POST variable is set
{
	$name = $_POST['name'];
	$email = $_POST['email'];
    $url = $_POST['url'];
	$message = $_POST['message'];
	
	$subject = "Email From Website: " . $name; // subject of the email msg
	
	$errors = array(); // empty array of the err
	
	/*
	 * The fields
	 * 1st param: submitted data
	 * 2nd param: reuqired (TRUE) or not (FALSE)  
	 * 3rd param: the name for the error
	*/
	$fields = array(
		'name'		=> array($name, TRUE, "Name"),
		'url' 	    => array($url, FALSE, "Website"),
		'email' 	=> array($email, TRUE, "E-mail Address"),
		'message' 	=> array($message, TRUE, "Your Message"),
	);
	
	$i=0;
	foreach ($fields as $key => $field) {
		if ( FALSE == test_field( $field[0], $field[1] ) ) {
			$errors[$key] = "The field '".$field[2]."' is required.";
		}
		$i++;
	}
	
	//var_dump($errors);
	
	if (empty($errors)) { // if there is no errors, we can send the mail
		$body = "";
		$body .= "----- Info about the sender -----\n\n";
		$body .= "Name: ".$fields['name'][0]."\n";
		$body .= "Email: ".$fields['email'][0]."\n";
		$body .= "Website: ".$fields['url'][0]."\n";
		$body .= "\n\n----- Message -----\n\n";
		$body .= $fields['message'][0];
		
		if( mail( $recipient, $subject, $body, "FROM: ".$fields['email'][0] ) ) { // try to send the message, if not successful, print out the error
			message_was_sent($fields);
		
        } else {
			echo "It is not possible to send the email. Check out your PHP settings!";
			print_the_form($errors, $fields);
		}
	} else { // if there are any errors
		print_the_form($errors, $fields);
	}	
} else {
	print_the_form();
}

/**
 * prints out the form if necessary
 */
function print_the_form($errors = array(), $fields = null) {
	?> 
    					<!--  = contact form =  -->
    					<form method="post" action="#" class="form form-inline form-contact">
                            <div class="control-group push-down-20<?php error_class('name', $errors); ?>">
                                <input type="text" aria-required="true" tabindex="1" size="30" value="<?php inpt_value('name', $fields); ?>" id="name" name="name" required>
                                <label for="name" class="control-label">Name<span class="red-clr bold">*</span></label>
                                <?php show_error('name', $errors); ?>
                            </div>
                            
                            <div class="control-group push-down-20<?php error_class('email', $errors); ?>">
                                <input type="email" aria-required="true" tabindex="2" size="30" value="<?php inpt_value('email', $fields); ?>" id="email" name="email" required>
                                <label for="email">Mail<span class="red-clr bold">*</span></label>
                                <?php show_error('email', $errors); ?>
                            </div>
                            
                            <div class="control-group push-down-20<?php error_class('url', $errors); ?>">
                                <input type="text" tabindex="3" size="30" value="<?php inpt_value('url', $fields); ?>" id="url" name="url">
                                <label for="url">Website</label>
                                <?php show_error('url', $errors); ?>
                            </div>
    
                            <div class="control-group push-down-20<?php error_class('message', $errors); ?>">
                                <textarea class="input-block-level" tabindex="4" rows="7" cols="70" id="message" name="message" placeholder="Your Message goes here ..." required><?php inpt_value('message', $fields); ?></textarea>
                                <?php show_error('message', $errors); ?>
                            </p>
                            <p>
                                <input type="hidden" value="1" name="submit" />
                                <button class="btn btn-primary bold" type="submit" tabindex="5" id="submit">SEND EMAIL</button>
                            </p>
                        </form>
                        <!--  = /contact form =  -->

	
	<?php
}

function message_was_sent($fields) { // notification that sending the mail was successful
	?> 
	<p class="text-info">Your message was sent successfully!</p>
	<?php
}

/**
 * Returns TRUE if field is required and OK
 */
function test_field($content, $required) {
	if ( TRUE === $required ) {
	    return strlen($content) > 0;
        
	} else {
		return TRUE;
	}
}

/**
 * Add the appropirate class name to the specified input field
 */
function error_class($name, $errors) {
	if ( array_key_exists( $name, $errors ) ) {
		echo " error";
	}
}

/**
 * repopulate the data when the form is submitted and errors returned
 */
function inpt_value($name, $fields) {
	if ( null === $fields ) {
		return;
	} else {
		echo $fields[$name][0];
	}
} 

function show_error( $name, $errors ) {
	if ( array_key_exists( $name, $errors ) )
	echo '<div class="help-block"> ' . $errors[$name] . ' </div>';
}
