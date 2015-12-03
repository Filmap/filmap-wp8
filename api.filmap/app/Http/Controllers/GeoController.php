<?php

namespace App\Http\Controllers;
use App\Geo;
use App\Film;
use DB;

class GeoController extends Controller
{
    /**
     * Finds the nearest films. 
     * Using the Haversine formula.
     * Raw Query:
     *
     * @param  int  $id
     * @return Response
     */
    public function filmsAround($distance, $lat, $lng)
    {
        /*
         lat = 37, long = -122 coordinate
        */
        
        // $lat = intval($lat);
        // $lng = intval($lng);

        // $films = DB::table('films')
        //             ->join('geos', 'films.geo_id', '=', 'geos.id')
        //             ->select(DB::raw('geos.id, ( 6371 * acos( 
        //                             cos( radians(' . $lat . ') ) * cos( radians( lat ) ) 
        //                             * cos( radians( lng ) - radians(' . $lng . ') ) 
        //                             + sin( radians(' . $lat . ') ) 
        //                             * sin( radians( lat ) ) ) ) AS distance'))
        //             ->having('distance', '<', 25)
        //             ->orderBy('distance')
        //             ->take(20)
        //             ->get();

         // 37.386339,-122.085823
        // $films = DB::table('geos')
        //             ->whereBetween('lat', [$lat-0.000100, $lat+0.000100])
        //             ->whereBetween('lng', [$lng-0.000100, $lng+0.000100])
        //             ->take(20)
        //             ->get();

        $lat = intval($lat);
        $lng = intval($lng);
        $films = Film::near($distance, $lat, $lng)->get();

        return json_encode($films); 
    }


}