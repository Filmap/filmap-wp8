<?php

use App\User;
use App\Film;
use App\Geo;

class GeoTest extends TestCase
{
    /**
     * Tests user auth.
     *
     * @return void
     */


    public function testNear()
    {
        $response = $this->get('/near/50,37,-122')->seeJson([
                "distance" => 43.626375422058,
             ]);
    }
}
