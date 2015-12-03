<?php

use Illuminate\Database\Seeder;
use Illuminate\Database\Eloquent\Model;

use App\User;
use App\Film;
use App\Geo;

class DatabaseSeeder extends Seeder
{
    /**
     * Run the database seeds.
     *
     * @return void
     */
    public function run()
    {
        $faker = Faker\Factory::create();

        Model::unguard();

        $user = new User([
            'name' => $faker->name,
            'email' => $faker->email,
            'password' => bcrypt('secret'),
        ]);

        $user->save();

        // Geo table
        $coordinates = [
            [37.386339,-122.085823],
            [37.38714,-122.083235],
            [37.393885,-122.078916],
            [37.402653,-122.079354],
            [37.394011, -122.095528],
            [37.401724,-122.114646],
        ];

        for ($i=0; $i < count($coordinates); $i++) { 

            $film = new Film([
                    'omdb' => $faker->randomDigitNotNull,
                    'user_id' => $user->id,
                    'watched' => $faker->boolean(),
                ]);

            $film->save();

            $geo = new Geo([
                'lat' => $coordinates[$i][0],
                'lng' => $coordinates[$i][1],
                'film_id' => $film->id,
            ]);

            $geo->save();

        }

        Model::reguard();
    }
}
