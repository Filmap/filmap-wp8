<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class Geo extends Model
{
    /**
	* The database table used by the model.
	*
	* @var string
	*/
    protected $table = 'geos';

    /**
	* The attributes that are mass assignable.
	*
	* @var array
	*/
	protected $fillable = ['lat', 'lng'];

	/*
	* Relationships
	*/

	public function film()
	{
		return $this->belongsTo('App\Film');
	}

	/*
	Queries
	*/
}