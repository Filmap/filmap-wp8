<?php

namespace App;

use Illuminate\Database\Eloquent\Model;
use DB;

class Film extends Model
{
    /**
	* The database table used by the model.
	*
	* @var string
	*/
    protected $table = 'films';

    /**
	* The attributes that are mass assignable.
	*
	* @var array
	*/
	protected $fillable = ['omdb', 'user_id', 'watched'];

	/*
	* Relationships
	*/

	public function user()
	{
		return $this->belongsTo('App\User');
	}

	public function geo()
	{
		return $this->hasOne('App\Geo');
	}

	public function scopeNear($query, $distance, $lat, $lng)
	{
		// return $this->geo()->near($distance, $lat, $lng);
		return $query->join('geos', 'films.id', '=', 'geos.film_id')
					->select(DB::raw('films.omdb, geos.lat, geos.lng,
										( 6371 * acos( 
											cos( radians(' . $lat . ') ) * cos( radians( lat ) ) 
											* cos( radians( lng ) - radians(' . $lng . ') ) 
											+ sin( radians(' . $lat . ') ) 
											* sin( radians( lat ) ) ) )
										AS distance '
									))
					->having('distance', '<', $distance)
					->orderBy('distance');
	}
}